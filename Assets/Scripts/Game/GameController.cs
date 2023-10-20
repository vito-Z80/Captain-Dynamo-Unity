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


        private const string LevelPrefPath = "Level/Level_";

        private void Start()
        {
            NextLevel();
            // data.Reset();
            // data.CollectLevel();
        }

        // private void FixedUpdate()
        // {
        //     if (data.HasLevelUpdate()) NextLevel();
        // }


        private void NextLevel()
        {
            var levelNumber = data.currentLevel;
            if (levelController is not null) Destroy(levelController.gameObject);
            // data.Reset();
            data.currentLevel = levelNumber;
            var levelName = LevelPrefPath + levelNumber;
            Debug.Log(levelName);
            var levelPref = Resources.Load<GameObject>(levelName);
            levelController = Instantiate(levelPref).GetComponent<LevelController>();
        }
    }
}