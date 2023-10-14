using System;
using Game.Actions;
using Game.Drops;
using Game.Platforms;
using UnityEngine;

namespace Game
{
    public class HeroHandler
    {
        private const string Enemy = "Enemy";
        private const string Deadly = "Deadly";
        private const string Drop = "Drop";
        private const string Platform = "Platform";
        private const string Ground = "Ground";
        private const string Exit = "Exit";

        

        public void OnTrigger(HeroController hero, Collider2D other)
        {
            if (!hero.isActive) return;
            if (other.gameObject.CompareTag(Enemy))
            {
                if (hero._rb.velocity.y < 0.0f)
                {
                    other.GetComponent<IDeath>().ToKill();
                    hero._rb.velocity = new Vector2(hero._rb.velocity.x, 0.0f);
                    hero.Jump(24.0f);
                }
                else
                {
                    hero.Dead();
                }
            }

            //
            if (other.gameObject.CompareTag(Deadly))
            {
                hero.Dead();
            }

            if (other.gameObject.CompareTag(Drop))
            {
                var d = other.GetComponent<ICollected>();
                d.Collect();
            }
        }

       

        public void OnCollision(HeroController hero, Collision2D other)
        {
            if (other.gameObject.CompareTag(Ground))
            {
                hero.isJumping = false;
            }

            if (other.gameObject.CompareTag(Platform))
            {
                hero.isJumping = false;
                if (other.gameObject.name.Contains(Exit))
                {
                    hero.LevelCompleted(other.transform.position);
                }
            }
        }
    }
}