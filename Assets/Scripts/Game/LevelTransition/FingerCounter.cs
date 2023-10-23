using System;
using System.Collections;
using UnityEngine;

namespace Game.LevelTransition
{
    public class FingerCounter : MonoBehaviour
    {
        public Sprite[] fingers;
        public SpriteRenderer fingersSpriteRenderer;


        private void Start()
        {
            fingersSpriteRenderer.enabled = false;
        }


        public IEnumerator Counter(Action done)
        {
            yield return new WaitForSeconds(1f);
            fingersSpriteRenderer.enabled = true;
            var count = 0;
            while (count < fingers.Length)
            {
                fingersSpriteRenderer.sprite = fingers[count];
                yield return new WaitForSeconds(1f);
                count++;
            }

            done.Invoke();
        }
    }
}