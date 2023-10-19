﻿using System;
using System.Collections;
using UnityEngine;

namespace Game.Platforms
{
    public class Bumper : MonoBehaviour
    {
        public Sprite bumperOffSprite;
        public Sprite bumperOnSprite;
        public float bounceForce = 100f;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            SetSprite(bumperOffSprite);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            StartCoroutine(ActiveState(collision));
            var bounceDirection = (collision.gameObject.transform.position - transform.position).normalized;
            collision.rigidbody.velocity = bounceDirection * bounceForce;
        }


        private IEnumerator ActiveState(Collision2D collision)
        {
            SetSprite(bumperOnSprite);
            var hc = collision.gameObject.GetComponent<HeroController>();
            hc.isActive = false;
            yield return new WaitForSeconds(0.2f);
            hc.isActive = true;
            SetSprite(bumperOffSprite);
            yield return null;
        }

        private void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
    }
}