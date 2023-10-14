using System;
using Camera;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// start next level
    /// finish level
    /// 
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [HideInInspector] [CanBeNull] public LevelController levelController = null;
        public GameData data;
        public CameraController cameraController;

        //  TODO следующий уровень выызывается но тут же заканчивается потому что перс находится в телепорте завершения уровня.


        private void Start()
        {
            data.CollectLevel();
        }

        private void FixedUpdate()
        {
            if (data.HasLevelUpdate()) NextLevel();
        }


        public void NextLevel()
        {
            var levelNumber = data.currentLevel;
            if (levelController is not null) Destroy(levelController.gameObject);
            data.Reset();
            data.currentLevel = levelNumber;
            var levelName = "Level/Level_" + levelNumber;
            Debug.Log(levelName);
            var levelPref = Resources.Load<GameObject>(levelName);
            levelController = Instantiate(levelPref).GetComponent<LevelController>();
        }
    }
}