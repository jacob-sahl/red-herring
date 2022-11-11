using System;
using System.Collections.Generic;

namespace Database
{
    [Serializable]
    public class Player
    {
        public int id;
        public string name;
        public int score;
        public bool joined;
        
        public Player(int id, string name)
        {
            this.id = id;
            this.name = name;
            score = 0;
            joined = false;
        }
        
        public void Join()
        {
            joined = true;
        }
        
        public void AddScore(int score)
        {
            this.score += score;
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
            foreach (RoundScore roundScore in scores)
            {
                if (roundScore.playerId == playerId)
                {
                    roundScore.score = score;
                    return;
                }
            }
            scores.Add(new RoundScore{playerId = playerId, score = score});
        }
        
        public void SetInformant(int playerId, string clue, string secretGoal)
        {
            foreach (InformantCard informantCard in informants)
            {
                if (informantCard.playerId == playerId)
                {
                    informantCard.clue = clue;
                    informantCard.secretGoal = secretGoal;
                    return;
                }
            }
            informants.Add(new InformantCard{playerId = playerId, clue = clue, secretGoal = secretGoal});
        }
        
        public void SetDetective(int playerId)
        {
            detective = playerId;
        }
        
        public int GetScore(int playerId)
        {
            foreach (RoundScore roundScore in scores)
            {
                if (roundScore.playerId == playerId)
                {
                    return roundScore.score;
                }
            }
            return 0;
        }
        
        
    }
    
    [Serializable]
    public class GameInstance
    {
        public string id;
        public string joinCode;
        public string createdTime;
        public List<Player> players = new List<Player>();
        public List<Round> rounds = new List<Round>();
        public int currentRound;

        public GameInstance()
        {
            id = Guid.NewGuid().ToString();
            joinCode = GenerateJoinCode();
            createdTime = DateTime.Now.ToString();
            currentRound = 0;
        }

        private string GenerateJoinCode()
        {
            // WARNING: This is not a secure way to generate a join code, but it's good enough for this project
            string joinCode = "";
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                joinCode += random.Next(0, 9);
            }

            return joinCode;
        }
        
        public void EndRound()
        {
            foreach (Player player in players)
            {
                player.AddScore(rounds[currentRound].GetScore(player.id));
            }
            currentRound++;
        }
        
        public void AddPlayer(int playerId, string name)
        {
            players.Add(new Player(playerId, name));
        }
        
        public int GetPlayerScore(int playerId)
        {
            foreach (Player player in players)
            {
                if (player.id == playerId)
                {
                    return player.score;
                }
            }
            return 0;
        }
        
        public void EndGame()
        {
            joinCode = "";
            currentRound = -1;
        }
        
        public bool AllPlayersJoined()
        {
            foreach (Player player in players)
            {
                if (!player.joined)
                {
                    return false;
                }
            }
            return true;
        }
    }
}