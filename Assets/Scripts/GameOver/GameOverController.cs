using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameOver
{
    public class GameOverController : MonoBehaviour
    {
        public UnityEngine.UI.Text gameOverText;

        private float _timer = 10.0f;

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0.0f) SceneManager.LoadScene("Scenes/MainMenu");
            InputListener();
            var pos = gameOverText.rectTransform.anchoredPosition;
            if (pos.y <= -64.0f) return;
            pos.y -= Time.deltaTime * 64.0f;
            gameOverText.rectTransform.anchoredPosition = pos;
        }


        private void InputListener()
        {
            if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("MenuKeyFire"))
            {
                SceneManager.LoadScene("Scenes/MainMenu");
            }
        }
    }
}