using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Game/Game Data")]
    public class GameData : ScriptableObject
    {

        public int diamondsOnLevel = 0;
        public int diamondsCollected = 0;
        [FormerlySerializedAs("scores")] public int score = 0;
        public int currentLevel = 0;
        public bool isMusicPlayed = true;
        public int deaths = 0;
        public int lives = 3;
        public GameMode gameMode = GameMode.Modern;

        public bool hasDiamondUpdate = true;
        public bool hasScoresUpdate = true;
        public bool hasLevelUpdate = true;

        public void Reset()
        {
            diamondsOnLevel = 0;
            diamondsCollected = 0;
            score = 0;
            currentLevel = 1;
            deaths = 0;
            lives = 3;
            hasScoresUpdate = true;
            hasDiamondUpdate = true;
            hasLevelUpdate = false;
        }

        public void CollectScores(int itemScores)
        {
            score += itemScores;
            hasScoresUpdate = true;
        }

        public void CollectDiamond()
        {
            diamondsCollected++;
            hasDiamondUpdate = true;
        }

        public void CollectLevel()
        {
            currentLevel++;
            hasLevelUpdate = true;
        }

        public bool HasDiamondUpdate()
        {
            if (!hasDiamondUpdate) return false;
            hasDiamondUpdate = false;
            return true;
        }

        public bool HasScoresUpdate()
        {
            if (!hasScoresUpdate) return false;
            hasScoresUpdate = false;
            return true;
        }

        public bool HasLevelUpdate()
        {
            if (!hasLevelUpdate) return false;
            hasLevelUpdate = false;
            return true;
        }
    }
}