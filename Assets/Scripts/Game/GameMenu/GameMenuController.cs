using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameMenu
{
    public class GameMenuController : MonoBehaviour
    {
    
        [HideInInspector] public GameController gameController;
        public UnityEngine.UI.Text onOffLabel;
        public UnityEngine.UI.Text toMainMenuLabel;
        public UnityEngine.UI.Text backLabel;

        public Transform selectorImage;

        private const string MOn = "On";
        private const string MOff = "Off";

        private int _vertPress = 0;
        private int _firePress = 0;
        private int _keyPressed = 0;
        
        private int _selectorIndex = 0;

        private UnityEngine.UI.Text[] _labels;

        private void Awake()
        {
            _labels = new[] { onOffLabel, toMainMenuLabel, backLabel };
        }

        private void Start()
        {
            if (gameController.gameMusic.isPlaying) onOffLabel.text = MOn;
            else onOffLabel.text = MOff;
            _selectorIndex = _labels.Length - 1;
            SetSelector(_selectorIndex);
        }

        private void OnEnable()
        {
            // _keyPressed = 0;
            // _selectorIndex = _labels.Length - 1;
            // SetSelector(_selectorIndex);
        }


        private void Update()
        {
            // var key = Input.GetAxis("Options");
            // var pressed = (int)(key > 0.0f ? 1.0f : key < 0.0f ? -1.0f : 0.0f);
            // if (pressed > 0 && pressed != _keyPressed)
            // {
            //     CloseMenu();
            // }
            // _keyPressed = pressed;
        }

        private void FixedUpdate()
        {
            MoveSelector();
            Select();
        }

        // public void ShowMenu(GameController gameController)
        // {
        //     this.gameController.levelController.heroController.gameObject.SetActive(false);
        //     gameObject.SetActive(true);
        //     _selectorIndex = _labels.Length - 1;
        //     SetSelector(_selectorIndex);
        // }

        private void Select()
        {
            var fire = Input.GetAxis("Select");
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
            if (gameController.gameMusic.isPlaying)
            {
                gameController.data.isMusicPlayed = false;
                gameController.gameMusic.Pause();
                tm.text = MOff;
            }
            else
            {
                gameController.data.isMusicPlayed = true;
                gameController.gameMusic.UnPause();
                tm.text = MOn;
            }
        }

        public void CloseMenu()
        {
            gameObject.SetActive(false);
            gameController.levelController.heroController.gameObject.SetActive(true);
        }
    }
}