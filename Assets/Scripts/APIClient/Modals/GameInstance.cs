using System;
using System.Collections.Generic;
using FullSerializer;

namespace APIClient
{
    [Serializable]
    public class Player
    {
        public int id;
        public string name;
        public int score;
        public string session;
        public bool isDetective;
        public string gameId;
        public void AddScore(int score)
        {
            this.score += score;
        }
        
        public Player(int id, string name, string gameId)
        {
            this.name = name;
            this.session = "DEBUG";
            this.gameId = gameId;
            this.id = id;
            this.score = 0;
            this.isDetective = false;
        }
        
    }

    [Serializable]
    public class InformantCard
    {
        public int playerId;
        public string clue;
        public string secretGoal;
    }

    [Serializable]
    public class RoundScore
    {
        public int playerId;
        public int score;
    }

    [Serializable]
    public class Round
    {
        public int roundNumber;
        public List<RoundScore> scores;
        public int detective;
        public List<InformantCard> informants;

        public Round(int roundNumber, int detective, List<InformantCard> informants, List<RoundScore> scores)
        {
            this.roundNumber = roundNumber;
            this.detective = detective;
            this.informants = informants;
            this.scores = scores;
        }

        public void SetScore(int playerId, int score)
        {
            foreach (var roundScore in scores)
                if (roundScore.playerId == playerId)
                {
                    roundScore.score = score;
                    return;
                }

            scores.Add(new RoundScore { playerId = playerId, score = score });
        }

        public void SetInformant(int playerId, string clue, string secretGoal)
        {
            foreach (var informantCard in informants)
                if (informantCard.playerId == playerId)
                {
                    informantCard.clue = clue;
                    informantCard.secretGoal = secretGoal;
                    return;
                }

            informants.Add(new InformantCard { playerId = playerId, clue = clue, secretGoal = secretGoal });
        }

        public void SetDetective(int playerId)
        {
            detective = playerId;
        }

        public int GetScore(int playerId)
        {
            foreach (var roundScore in scores)
                if (roundScore.playerId == playerId)
                    return roundScore.score;
            return 0;
        }
    }

    [Serializable]
    public class GameInstance
    {
        public string id;
        public string joinCode;
        public string createdTime;
        public List<Player> players = new();
        public List<Round> rounds = new();
        public int currentRound;

        private string GenerateJoinCode()
        {
            // WARNING: This is not a secure way to generate a join code, but it's good enough for this project
            var joinCode = "";
            var random = new Random();
            for (var i = 0; i < 4; i++) joinCode += random.Next(0, 9);

            return joinCode;
        }

        public void EndRound()
        {
            foreach (var player in players) player.AddScore(rounds[currentRound].GetScore(player.id));
            currentRound++;
        }


        public int GetPlayerScore(int playerId)
        {
            foreach (var player in players)
                if (player.id == playerId)
                    return player.score;
            return 0;
        }

        public void EndGame()
        {
            joinCode = "";
            currentRound = -1;
        }
        
        public bool Equals(GameInstance other)
        {
            fsSerializer serializer = new();
            serializer.TrySerialize(this, out fsData data1);
            serializer.TrySerialize(other, out fsData data2);
            return data1.Equals(data2);
        }
        
    }
}