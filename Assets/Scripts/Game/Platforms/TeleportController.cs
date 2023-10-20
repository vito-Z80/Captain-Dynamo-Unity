using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game.Platforms
{
    public class TeleportController : MonoBehaviour
    {
        public GameObject startTeleport;
        public GameObject finishTeleport;

        public GameObject spriteLinePref;
        private readonly List<GameObject> _lines = new List<GameObject>();

        public static event Action LevelCompleted;


        public GameData gameData;



        private void Awake()
        {
            CreateLines();
        }

        
        public void StartLevel(HeroController heroController)
        {
            StartCoroutine(Launch(heroController));
        }

        public void FinishLevel(HeroController heroController)
        {
            StartCoroutine(Finish(heroController));
        }


        private IEnumerator Launch(HeroController heroController)
        {
            // yield return new WaitUntil(() => gameObject.activeSelf);
            var startTeleportPosition = startTeleport.transform.position;
            heroController.SetStartPosition(startTeleportPosition + (Vector3.up * 16.0f));
            heroController.isActive = false;
            heroController.gameObject.SetActive(false);
            SetLinesPositions(startTeleportPosition);
            yield return new WaitForSeconds(1.0f);
            foreach (var line in _lines)
            {
                yield return new WaitForSeconds(0.15f);
                line.SetActive(true);
            }

            //  show HERO
            heroController.gameObject.SetActive(true);

            foreach (var line in _lines)
            {
                yield return new WaitForSeconds(0.15f);
                line.SetActive(false);
            }

            yield return null;
            //  start play
            heroController.isActive = true;
        }


        private IEnumerator Finish(HeroController heroController)
        {
            SetLinesPositions(finishTeleport.transform.position);
            yield return new WaitForSeconds(0.5f);
            foreach (var line in _lines)
            {
                yield return new WaitForSeconds(0.15f);
                line.SetActive(true);
            }

            //  hide HERO
            heroController.gameObject.SetActive(false);

            foreach (var line in _lines)
            {
                yield return new WaitForSeconds(0.15f);
                line.SetActive(false);
            }

            yield return new WaitForSeconds(1.0f);
            yield return null;
            //  TODO go to next level
            // LevelCompleted?.Invoke();
            gameData.CollectLevel();
            SceneManager.LoadScene("Scenes/LevelTransition");
        }

        private void SetLinesPositions(Vector3 position)
        {
            var topPos = position.y + 9.0f;
            for (var y = 0; y < 16; y++)
            {
                var value = y * 2;
                if (value >= 16) value -= 15;
                var resultPos = new Vector3(position.x, value + topPos, 0.0f);
                _lines[y].transform.position = resultPos;
            }

            _lines.Reverse(0, _lines.Count);
        }


        private void CreateLines()
        {
            for (var y = 0; y < 16; y++)
            {
                var spriteObject = Instantiate(spriteLinePref, Vector3.zero, Quaternion.identity, transform);
                spriteObject.SetActive(false);
                _lines.Add(spriteObject);
            }
        }
    }
}