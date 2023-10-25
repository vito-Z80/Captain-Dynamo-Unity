using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameMenu
{
    public class GameMenuController : MonoBehaviour
    {
        private GameController _gameController;
        public UnityEngine.UI.Text onOffLabel;
        public UnityEngine.UI.Text toMainMenuLabel;
        public UnityEngine.UI.Text backLabel;

        public Transform selectorImage;

        private const string MOn = "On";
        private const string MOff = "Off";

        private int _vertPress = 0;
        private int _firePress = 0;

        private int _selectorIndex = 0;

        private UnityEngine.UI.Text[] _labels;

        private void Awake()
        {
            _labels = new[] { onOffLabel, toMainMenuLabel, backLabel };
            if (_gameController.gameMusic.isPlaying) onOffLabel.text = MOn;
            else onOffLabel.text = MOff;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) CloseMenu();
        }

        private void FixedUpdate()
        {
            MoveSelector();
            Select();
        }

        public void ShowMenu(GameController gameController)
        {
            _gameController = gameController;
            gameObject.SetActive(true);
            _selectorIndex = _labels.Length - 1;
            SetSelector(_selectorIndex);
            _gameController.levelController.heroController.gameObject.SetActive(false);
        }

        private void Select()
        {
            var fire = Input.GetAxis("Jump");
            var pressed = (int)(fire > 0.0f ? 1.0f : fire < 0.0f ? -1.0f : 0.0f);
            if (pressed > 0 && pressed != _firePress)
            {
                _labels[_selectorIndex].GetComponent<SelectorInvoke>().Execute();
            }

            _firePress = pressed;
        }


        private void SetSelector(int selectorIndex)
        {
            selectorImage.position = _labels[selectorIndex].transform.position;
        }

        private void MoveSelector()
        {
            var vAxis = Input.GetAxis("VerticalMenu");
            var vert = (int)(vAxis > 0.0f ? 1.0f : vAxis < 0.0f ? -1.0f : 0.0f);

            if (vert > 0 && vert != _vertPress)
            {
                _selectorIndex = Mathf.Clamp(--_selectorIndex, 0, _labels.Length - 1);
                SetSelector(_selectorIndex);
            }

            if (vert < 0 && vert != _vertPress)
            {
                _selectorIndex = Mathf.Clamp(++_selectorIndex, 0, _labels.Length - 1);
                SetSelector(_selectorIndex);
            }

            _vertPress = vert;
        }

        public void ToMainMenu()
        {
            SceneManager.LoadScene("Scenes/MainMenu");
        }

        public void ChangeMusicState()
        {
            var tm = _labels.First(mesh => mesh.name.Contains("off", StringComparison.OrdinalIgnoreCase));
            if (_gameController.gameMusic.isPlaying)
            {
                _gameController.data.isMusicPlayed = false;
                _gameController.gameMusic.Pause();
                tm.text = MOff;
            }
            else
            {
                _gameController.data.isMusicPlayed = true;
                _gameController.gameMusic.UnPause();
                tm.text = MOn;
            }
        }

        public void CloseMenu()
        {
            gameObject.SetActive(false);
            _gameController.levelController.heroController.gameObject.SetActive(true);
        }
    }
}