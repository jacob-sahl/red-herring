using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class TestDatabase : MonoBehaviour
    {
        private void Start()
        {
            // GameInstance gameInstance = new GameInstance();
            // gameInstance.players.Add(new Player(0, "Player 0"));
            // gameInstance.players.Add(new Player(1, "Player 1"));
            // gameInstance.players.Add(new Player(2, "Player 2"));
            // gameInstance.players.Add(new Player(3, "Player 3"));
            //
            // gameInstance.rounds.Add(new Round(0, 0, new List<InformantCard>
            // {
            //     new InformantCard { playerId = 1, clue = "Clue 1", secretGoal = "Secret Goal 1" },
            //     new InformantCard { playerId = 2, clue = "Clue 2", secretGoal = "Secret Goal 2" },
            //     new InformantCard { playerId = 3, clue = "Clue 3", secretGoal = "Secret Goal 3" }
            // }, new List<RoundScore>
            // {
            //     new RoundScore { playerId = 0, score = 0 },
            //     new RoundScore { playerId = 1, score = 0 },
            //     new RoundScore { playerId = 2, score = 0 },
            //     new RoundScore { playerId = 3, score = 0 },
            // }));
            //
            // DatabaseHandler.PostGame(gameInstance, gameInstance.id, () => { Debug.Log("Posted"); });
            // DatabaseHandler.GetGameInstance("4fc7d935-0952-4903-ac42-452630cae765", instance =>
            // {
            // Debug.Log(instance);
            // });
            var readyGameInstance = DatabaseHandler.WaitForAllPlayersToJoin("4fc7d935-0952-4903-ac42-452630cae765");
            Debug.Log(readyGameInstance);
        }
    }
}