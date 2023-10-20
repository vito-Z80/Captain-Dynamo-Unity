using System;
using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelTransition
{
    public class AfterLevel : MonoBehaviour
    {
        public GameData gameData;
        public TextMesh perfectLabel;
        public TextMesh diamonds;
        public TextMesh score;
        [HideInInspector] public bool isDone = false;

        private const int DiamondPrice = 1000;
        private const int PerfectBonus = 10000;

        private const string D8 = "D8";
        private const string D2 = "D2";

        private readonly Color[] _flash = new []
        {
            Color.blue, 
            Color.red, 
            Color.magenta, 
            Color.green, 
            Color.cyan, 
            Color.yellow, 
            Color.white, 
        };

        private int _flashCount = 0;
        private float _flashTimer;

        private void Start()
        {
            PrintScore();
            PrintDiamonds();
            StartCoroutine(CountDiamonds());
        }


        private void Update()
        {
            if ((_flashTimer += Time.deltaTime) < 0.1f) return;
            _flashTimer = 0.0f;
            perfectLabel.color = _flash[_flashCount++];
            if (_flashCount < _flash.Length) return;
            _flashCount = 0;
        }


        private IEnumerator CountDiamonds()
        {
            var isPerfect = gameData.diamondsCollected == gameData.diamondsOnLevel;
            while (gameData.diamondsCollected > 0)
            {
                yield return new WaitForSeconds(0.075f);
                gameData.diamondsCollected--;
                gameData.scores += DiamondPrice;
                PrintDiamonds();
                PrintScore();
            }

            yield return new WaitForSeconds(1f);
            if (isPerfect)
            {
                perfectLabel.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                gameData.scores += PerfectBonus;
                PrintScore();
            }
            yield return new WaitForSeconds(3f);
            isDone = true;
        }


        private void PrintScore()
        {
            score.text = gameData.scores.ToString(D8);
        }

        private void PrintDiamonds()
        {
            diamonds.text = gameData.diamondsCollected.ToString(D2);
        }
    }
}