using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Game/Game Data")]
    public class GameData : ScriptableObject
    {
        public int diamondsOnLevel = 0;
        public int diamondsCollected = 0;
        public int scores = 0;
        public int currentLevel = 0;

        public bool hasDiamondUpdate = true;
        public bool hasScoresUpdate = true;
        public bool hasLevelUpdate = true;

        public void Reset()
        {
            diamondsOnLevel = 0;
            diamondsCollected = 0;
            scores = 0;
            currentLevel = 0;
            hasScoresUpdate = false;
            hasDiamondUpdate = false;
            hasLevelUpdate = false;
        }

        public void CollectScores(int itemScores)
        {
            scores += itemScores;
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