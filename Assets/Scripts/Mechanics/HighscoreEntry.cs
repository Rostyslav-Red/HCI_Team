using System;
namespace Platformer.Gameplay {
    
    [Serializable]
    public class HighScoreEntry
    {
        public int score;
        public string playerName;

        public HighScoreEntry(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }
}