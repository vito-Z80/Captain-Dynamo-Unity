using System;
using Camera;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [HideInInspector] [CanBeNull] public LevelController levelController = null;

        public bool debugLevel;

        public GameData data;
        public CameraController cameraController;


        private void Start()
        {
            data.Reset();
            data.CollectLevel();
        }

        private void FixedUpdate()
        {
            if (data.HasLevelUpdate()) NextLevel();
        }


        private void NextLevel()
        {
            var levelNumber = data.currentLevel;
            if (levelController is not null) Destroy(levelController.gameObject);
            data.Reset();
            data.currentLevel = levelNumber;
            var levelName = "Level/Level_" + levelNumber;
            if (debugLevel)
            {
                levelController = GameObject.Find("Level_").GetComponent<LevelController>();
            }
            else
            {
                var levelPref = Resources.Load<GameObject>(levelName);
                levelController = Instantiate(levelPref).GetComponent<LevelController>();
            }
        }
    }
}