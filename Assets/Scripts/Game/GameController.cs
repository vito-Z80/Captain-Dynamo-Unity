using Game.GameMenu;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [HideInInspector] [CanBeNull] public LevelController levelController = null;


        public GameData data;

        public GameMenuController gameMenuController;

        [HideInInspector] public AudioSource gameMusic;
        private const string LevelPrefPath = "Level/Level_";

        private int _keyPressed = 0;

        private void Start()
        {
            gameMenuController.gameObject.SetActive(false);
            gameMenuController.gameController = this;
            gameMusic = GetComponent<AudioSource>();
            NextLevel();
        }


        private void Update()
        {
            var key = Input.GetAxis("Options");
            var pressed = (int)(key > 0.0f ? 1.0f : key < 0.0f ? -1.0f : 0.0f);
            if (pressed > 0 && pressed != _keyPressed)
            {
                if (levelController is not null && levelController.heroController.IsActive())
                {
                    gameMenuController.gameObject.SetActive(!gameMenuController.gameObject.activeSelf);
                    levelController.heroController.gameObject.SetActive(!gameMenuController.gameObject.activeSelf);
                }
            }
            _keyPressed = pressed;
        }

        private void NextLevel()
        {
            gameMusic.Play();
            if (!data.isMusicPlayed) gameMusic.Pause();
            var levelNumber = data.currentLevel;
            if (levelController is not null) Destroy(levelController.gameObject);
            data.currentLevel = levelNumber;
            var levelName = LevelPrefPath + levelNumber;
            var levelPref = Resources.Load<GameObject>(levelName);
            levelController = Instantiate(levelPref).GetComponent<LevelController>();
        }
    }
}