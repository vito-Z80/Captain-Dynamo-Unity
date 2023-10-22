using System;
using System.Collections;
using UnityEngine;

namespace Game.LevelTransition
{
    public class FingerCounter : MonoBehaviour
    {
        public Sprite[] fingers;
        public SpriteRenderer fingersSpriteRenderer;


    

        public IEnumerator Counter(Action done)
        {
            var count = 0;
            while (count < fingers.Length)
            {
                if (Input.anyKey) break;
                yield return new WaitForSeconds(1f);
                fingersSpriteRenderer.sprite = fingers[count];
                count++;
            }
            if (!Input.anyKey) yield return new WaitForSeconds(1f);
            done.Invoke();
        }
    }
}