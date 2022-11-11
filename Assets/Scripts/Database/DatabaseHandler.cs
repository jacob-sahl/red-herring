using System.Threading;
using FullSerializer;
using Models;
using Proyecto26;
using RSG;
using UnityEngine;
using UnityEngine.Networking;

namespace Database
{
    public class DatabaseHandler
    {
        private const string projectId = "red-herring-139ff";
        private static readonly string databaseURL = $"https://{projectId}-default-rtdb.firebaseio.com/";
        private static fsSerializer serializer = new fsSerializer();

        public delegate void PostGameCallback();

        public static IPromise<GameInstance> PostGame(GameInstance gameInstance, string gameID)
        {
            return RestClient.Put<GameInstance>($"{databaseURL}games/{gameID}.json", gameInstance);
        }

        public static IPromise<GameInstance> GetGameInstance(string gameID)
        {
            var promise = new Promise<GameInstance>();
            RestClient.Get($"{databaseURL}games/{gameID}.json").Then(response =>
            {
                var responseJson = response.Text;
                var data = fsJsonParser.Parse(responseJson);
                object deserialized = null;
                serializer.TryDeserialize(data, typeof(GameInstance), ref deserialized);
                var gameInstance = deserialized as GameInstance;
                
                promise.Resolve(gameInstance);
            }, exception =>
            {
                promise.Reject(exception);
            });
            return promise;
        }

        public static GameInstance WaitForAllPlayersToJoin(string gameID)
        {
            bool allPlayersJoined = false;
            GameInstance readyGameInstance = null;
            while (!allPlayersJoined)
            {
                GetGameInstance(gameID).Then(gameInstance =>
                {
                    if (gameInstance.AllPlayersJoined())
                    {
                        allPlayersJoined = true;
                        readyGameInstance = gameInstance;
                    }
                });
                Thread.Sleep(5000);
            }
            return readyGameInstance;
        }
    }
}