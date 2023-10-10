using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Platforms
{
    public class TeleportController : MonoBehaviour
    {
        public GameObject startTeleport;
        public GameObject finishTeleport;

        public GameObject spriteLinePref;
        private readonly List<GameObject> _lines = new List<GameObject>();

        

        public Bounds finishArea;


        private void Awake()
        {
            CreateLines();

            finishArea = new Bounds(
                finishTeleport.transform.position + Vector3.up * 16.0f,
                new Vector3(8f, 8f, 0.0f)
            );
        }


        public bool OnLevelCompleted(HeroController heroPosition)
        {
            return finishArea.Contains(heroPosition.transform.position);
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
            heroController.isActive = false;
            heroController.gameObject.SetActive(false);
            SetLinesPositions(startTeleport.transform.position.y);
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
            heroController.isActive = false;
            SetLinesPositions(finishTeleport.transform.position.y);
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
        }

        private void SetLinesPositions(float positionY)
        {
            var topPos = positionY + 9.0f;
            for (var y = 0; y < 16; y++)
            {
                var value = y * 2;
                if (value >= 16) value -= 15;
                var resultPos = new Vector3(0.0f, value + topPos, 0.0f);
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