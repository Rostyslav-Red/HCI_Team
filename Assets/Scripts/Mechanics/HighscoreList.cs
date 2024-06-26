using System;
using System.Collections.Generic;
using System.Linq;
namespace Platformer.Gameplay {

    [Serializable]
    public class HighScoreList
    {
        public List<HighScoreEntry> highScores = new List<HighScoreEntry>();

        public void AddHighScore(int score, string playerName)
        {
            highScores.Add(new HighScoreEntry(score, playerName));
            highScores = highScores.OrderByDescending(hs => hs.score).Take(10).ToList();
        }

        public bool IsHighScore(int score)
        {
            if (highScores.Count < 10)
                return true;

            return highScores.Any(hs => score > hs.score);
        }
    }
}