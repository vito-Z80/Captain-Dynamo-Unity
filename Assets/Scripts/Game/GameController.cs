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

        private void Start()
        {
            gameMenuController.gameObject.SetActive(false);
            gameMusic = GetComponent<AudioSource>();
            NextLevel();
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && levelController is not null)
            {
                if (levelController is not null && levelController.heroController.IsActive())
                    gameMenuController.ShowMenu(this);
            }
        }

        public void ActivateHero()
        {
            levelController?.heroController.ActiveState(true);
        }

        public void DeactivateHero()
        {
            levelController?.heroController.ActiveState(false);
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