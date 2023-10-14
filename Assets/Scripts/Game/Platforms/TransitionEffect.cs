using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Platforms
{
    public class TransitionEffect : ScriptableObject
    {
        public GameObject spriteLinePref;
        private const int LineCount = 24;
        private readonly List<GameObject> _lines = new(LineCount);


        private IEnumerator FillArea()
        {
            
            
            yield return null;
        }

        
        private IEnumerable ClearArea()
        {

            while (true)
            {
                yield return false;
            }
            
            
            yield return true;
        }

        private IEnumerator Launch(HeroController heroController)
        {
            // yield return new WaitUntil(() => gameObject.activeSelf);
            heroController.isActive = false;
            heroController.gameObject.SetActive(false);
            // SetLinesPositions(startTeleport.transform.position.y);
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
            // SetLinesPositions(finishTeleport.transform.position.y);
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


        private void CreateLines(Transform transform)
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