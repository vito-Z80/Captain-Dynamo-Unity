using Game;
using UnityEngine;

namespace UI
{
    public class GameUiController : MonoBehaviour
    {
        public GameData gameData;
        public UnityEngine.UI.Text scoreValue;
        public UnityEngine.UI.Text diamondsValue;
        public GameObject helmetArea;

        public GameObject helmetPref;
        private GameObject[] _lives;
        private Vector3 _helmetPosition;


        private void Awake()
        {
            helmetArea.SetActive(gameData.gameMode == GameMode.Classic);
            var pos = helmetArea.transform.position;
            pos.x -= 128.0f;
            _helmetPosition = pos;
            _lives = new[]
            {
                Instantiate(helmetPref, GetNextHelmetPosition(), Quaternion.identity, helmetArea.transform),
                Instantiate(helmetPref, GetNextHelmetPosition(), Quaternion.identity, helmetArea.transform),
                Instantiate(helmetPref, GetNextHelmetPosition(), Quaternion.identity, helmetArea.transform),
                Instantiate(helmetPref, GetNextHelmetPosition(), Quaternion.identity, helmetArea.transform),
                Instantiate(helmetPref, GetNextHelmetPosition(), Quaternion.identity, helmetArea.transform)
            };
        }


        private void FixedUpdate()
        {
            Values();
            Lives();
        }


        private void Values()
        {
            scoreValue.text = gameData.score.ToString(Define.D8);
            diamondsValue.text = gameData.diamondsCollected.ToString(Define.D2);
        }

        private void Lives()
        {
            if (gameData.gameMode == GameMode.Modern) return;
            for (var id = 0; id < _lives.Length; id++)
            {
                _lives[id].SetActive(id < gameData.lives);
            }
        }

        private Vector3 GetNextHelmetPosition()
        {
            return _helmetPosition += Vector3.right * 16.0f;
        }
    }
}