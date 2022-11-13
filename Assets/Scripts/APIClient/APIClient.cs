using FullSerializer;
using JetBrains.Annotations;
using Proyecto26;
using RSG;

namespace APIClient
{
    public class APIClient
    {
        private static APIClient _instance;

        private static readonly string endpoint = "http://localhost:3000/api/";
        private static readonly string APIKey = "WbyRAZEDyxXvb3N4QETCWidCd3b3T";
        private readonly fsSerializer serializer = new();

        private APIClient()
        {
            RestClient.DefaultRequestHeaders["x-api-key"] = APIKey;
        }

        public static APIClient Instance
        {
            get
            {
                if (_instance == null) _instance = new APIClient();
                return _instance;
            }
        }

        private string GetUri(string path)
        {
            return endpoint + path;
        }

        private Promise<T> Get<T>(string path, object data) where T : class
        {
            var promise = new Promise<T>();
            var uri = GetUri(path);
            RestClient.Get(uri).Then(response =>
            {
                var responseJson = response.Text;
                var responseData = fsJsonParser.Parse(responseJson);
                object deserialized = null;
                serializer.TryDeserialize(responseData, typeof(T), ref deserialized);
                var responseObject = deserialized as T;

                promise.Resolve(responseObject);
            }).Catch(error => { promise.Reject(error); });
            return promise;
        }

        private Promise<T> Post<T>(string path, [CanBeNull] object data) where T : class
        {
            var promise = new Promise<T>();
            var uri = GetUri(path);
            RestClient.Post(uri, data).Then(response =>
            {
                var responseJson = response.Text;
                var responseData = fsJsonParser.Parse(responseJson);
                object deserialized = null;
                serializer.TryDeserialize(responseData, typeof(T), ref deserialized);
                var responseObject = deserialized as T;

                promise.Resolve(responseObject);
            }).Catch(error => { promise.Reject(error); });
            return promise;
        }

        public Promise<GameInstance> CreateGameInstance()
        {
            return Post<GameInstance>("admin/games/", null);
        }

        public Promise<GameInstance> GetGameInstance(string id)
        {
            return Get<GameInstance>($"admin/games/{id}", null);
        }
    }
}