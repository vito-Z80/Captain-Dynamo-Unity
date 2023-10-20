using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelTransition
{
    public class CavernTransition : MonoBehaviour
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
            }

            yield return null;
        }
    }
}