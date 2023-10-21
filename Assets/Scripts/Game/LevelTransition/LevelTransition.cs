using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.LevelTransition
{
    public class LevelTransition : MonoBehaviour
    {
        public GameData gameData;
        public AfterLevel afterLevel;
        public BeforeLevel beforeLevel;


        private void Start()
        {
            afterLevel.isDone = false;
            beforeLevel.isDone = false;
            StartCoroutine(Launch());
        }


        private IEnumerator Launch()
        {
            if (gameData.currentLevel > 1)
            {
                afterLevel.gameObject.SetActive(true);
                while (!afterLevel.isDone)
                {
                    yield return new WaitForSeconds(1f);
                }

                afterLevel.gameObject.SetActive(false);
            }

            if (gameData.currentLevel < 7)
            {
                beforeLevel.gameObject.SetActive(true);
                while (!beforeLevel.isDone)
                {
                    yield return new WaitForSeconds(1f);
                }

                beforeLevel.gameObject.SetActive(false);
                SceneManager.LoadScene("Scenes/Game");
                yield return null;
            } else if (gameData.currentLevel >= 7)
            {
                SceneManager.LoadScene("Scenes/MainMenu");
            }

            yield return null;
        }
    }
}