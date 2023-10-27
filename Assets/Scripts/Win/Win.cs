using Game;
using UnityEngine;

namespace Win
{
    public class Win : MonoBehaviour
    {
        public GameData gameData;
        public GameObject withoutDeathMessage;
        public GameObject withDeathMessage;
        public GameObject badGamer;
        public GameObject congratulationMessage;
        public GameObject anyKeyDown;
        public UnityEngine.UI.Text gameModeValue;
        public UnityEngine.UI.Text deaths;
        public UnityEngine.UI.Text score;


        private float _blinkTime;
        private CongratulationsText _congratulationsText;

        private void Start()
        {
            gameModeValue.text = gameData.gameMode.ToString();
            deaths.text = gameData.deaths.ToString();
            score.text = gameData.score.ToString();
            withoutDeathMessage.SetActive(gameData.deaths == 0);
            withDeathMessage.SetActive(gameData.deaths is > 0 and < 50);
            badGamer.SetActive(gameData.deaths >= 50);
            _congratulationsText = congratulationMessage.GetComponent<CongratulationsText>();
        }

        private void FixedUpdate()
        {
            AnyKeyShow();
        }

        private void AnyKeyShow()
        {
            if (!_congratulationsText.canPressAnyKey) return;
            _blinkTime += Time.fixedDeltaTime;
            if (_blinkTime < 0.5f) return;
            _blinkTime = 0.0f;
            anyKeyDown.SetActive(!anyKeyDown.activeSelf);
        }
    }
}