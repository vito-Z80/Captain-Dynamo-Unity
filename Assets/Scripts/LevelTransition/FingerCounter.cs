﻿using System;
using System.Collections;
using UnityEngine;

namespace LevelTransition
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
                yield return new WaitForSeconds(1f);
                fingersSpriteRenderer.sprite = fingers[count];
                count++;
            }

            yield return new WaitForSeconds(1f);
            done.Invoke();
        }
    }
}