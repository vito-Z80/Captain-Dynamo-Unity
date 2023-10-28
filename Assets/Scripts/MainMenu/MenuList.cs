using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MenuList : MonoBehaviour
    {
        public GameData gameData;
        public List<UnityEngine.UI.Text> labels;
        public List<UnityEvent> events;
        public GameObject selector;


        private int _selectorId = 1;
        private int _preDirection = 0;


        private void Start()
        {
            selector.transform.position = labels[_selectorId].transform.position;
            labels.Find(text => text.gameObject.name.Contains("Diff")).text = gameData.gameMode.ToString();
        }

        private void Update()
        {
            MoveSelector();
            EventHandler();
        }


        private void EventHandler()
        {
            if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("MenuKeyFire"))
                events[_selectorId].Invoke();
        }

        private void MoveSelector()
        {
            var direction = (int)Input.GetAxis("Vertical");
            if (_preDirection == direction) return;
            SetSelectorPlace(direction);
            _preDirection = direction;
        }

        private void SetSelectorPlace(int direction)
        {
            _selectorId = Mathf.Clamp(_selectorId + direction * -1, 0, labels.Count - 1);
            selector.transform.position = labels[_selectorId].transform.position;
        }


        public void ExitEvent()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer) Application.Quit();
        }

        public void GameModeEvent()
        {
            gameData.gameMode = gameData.gameMode == GameMode.Classic ? GameMode.Modern : GameMode.Classic;
            labels[_selectorId].text = gameData.gameMode.ToString();
        }

        public void StartPlayEvent()
        {
            gameData.Reset();
            SceneManager.LoadScene("Scenes/LevelTransition");
        }


        public void WaspUrl()
        {
            Application.OpenURL(Define.WaspUrl);
        }

        public void CodeMastersUrl()
        {
            Application.OpenURL(Define.CodeMastersUrl);
        }

        public void GameUrl()
        {
            Application.OpenURL(Define.GameUrl);
        }
    }
}