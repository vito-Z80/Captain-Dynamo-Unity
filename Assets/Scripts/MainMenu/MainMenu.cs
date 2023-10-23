using System;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public AudioSource anyKeySound;
        public GameData gameData;
        public TextMesh anyKeyLabel;

        private readonly Color[] _flash = new[]
        {
            Color.blue,
            Color.red,
            Color.magenta,
            Color.green,
            Color.cyan,
            Color.yellow,
            Color.white,
        };

        private float _flashTimer;
        private int _flashCount;


        private float _startTimer;
        private float _changeScreenTimer;

        private void Start()
        {
            _startTimer = 0.0f;
            _changeScreenTimer = 0.0f;
            _flashTimer = 0.0f;
            _flashCount = 0;
        }

        private void Update()
        {
            Flash();


            if ((_startTimer += Time.deltaTime) < 0.1f) return;

            if (Input.anyKeyDown && _changeScreenTimer < 1.0f)
            {
                anyKeySound.Play();
                gameData.Reset();
                _changeScreenTimer = 1.0f;
            }

            if (_changeScreenTimer < 1.0f) return;
            _changeScreenTimer += Time.deltaTime;
            if (_changeScreenTimer > 1.5f) SceneManager.LoadScene("Scenes/LevelTransition");
        }

        private void Flash()
        {
            if ((_flashTimer += Time.deltaTime) < 0.1f) return;
            _flashTimer = 0.0f;
            anyKeyLabel.color = _flash[_flashCount++];
            if (_flashCount < _flash.Length) return;
            _flashCount = 0;
        }
    }
}