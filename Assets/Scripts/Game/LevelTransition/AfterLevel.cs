﻿using System.Collections;
using UnityEngine;

namespace Game.LevelTransition
{
    public class AfterLevel : MonoBehaviour
    {

        public AudioSource audioSource;
        public GameData gameData;
        public TextMesh perfectLabel;
        public TextMesh diamonds;
        public TextMesh score;
        [HideInInspector] public bool isDone = false;

        private const int DiamondPrice = 1000;
        private const int PerfectBonus = 10000;

        private const string D8 = "D8";
        private const string D2 = "D2";


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
            perfectLabel.color = Define.colors[_flashCount++];
            if (_flashCount < Define.colors.Length) return;
            _flashCount = 0;
        }


        private IEnumerator CountDiamonds()
        {
            var isPerfect = gameData.diamondsCollected == gameData.diamondsOnLevel;
            yield return new WaitForSeconds(1f);
            while (gameData.diamondsCollected > 0)
            {
                audioSource.Play();
                yield return new WaitForSeconds(0.075f);
                gameData.diamondsCollected--;
                gameData.score += DiamondPrice;
                PrintDiamonds();
                PrintScore();
            }

            yield return new WaitForSeconds(1f);
            if (isPerfect)
            {
                perfectLabel.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                gameData.score += PerfectBonus;
                PrintScore();
            }
            yield return new WaitForSeconds(3f);
            isDone = true;
        }


        private void PrintScore()
        {
            score.text = gameData.score.ToString(D8);
        }

        private void PrintDiamonds()
        {
            diamonds.text = gameData.diamondsCollected.ToString(D2);
        }
    }
}