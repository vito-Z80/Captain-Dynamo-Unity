using Game.Actions;
using Game.Drops;
using UnityEngine;

namespace Game
{
    public class HeroHandler
    {

        private const float SlowingCoefficient = 2.5f;
        public void OnTrigger(HeroController hero, Collider2D other)
        {
            
            if (!hero.isActive) return;
            if (other.gameObject.CompareTag(Define.Enemy))
            {
                if (hero._rb.velocity.y < 0.0f && !other.name.Contains(Define.Shuriken))
                {
                    other.GetComponent<IDeath>().ToKill();
                    hero._rb.velocity = new Vector2(hero._rb.velocity.x, 0.0f);
                    hero.Jump(24.0f);
                    hero.killSound.Play();
                }
                else
                {
                    hero.Dead();
                }
            }

            //
            if (other.gameObject.CompareTag(Define.Deadly))
            {
                hero.Dead();
            }

            if (other.gameObject.CompareTag(Define.Drop))
            {
                var d = other.GetComponent<ICollected>();
                hero.diamondSound.Play();
                d.Collect();
            }
        }


        public void OnCollision(HeroController hero, Collision2D other)
        {
            if (other.gameObject.CompareTag(Define.Ground))
            {
                hero.isJumping = false;
            }

            if (other.gameObject.CompareTag(Define.Platform))
            {
                hero.isJumping = false;
                if (other.gameObject.name.Contains(Define.Exit))
                {
                    hero.levelController.LevelCompleted();
                }

                if (other.gameObject.name.Contains(Define.Slowing))
                {
                    hero.speed /= SlowingCoefficient;
                }
            }

            if (other.gameObject.CompareTag(Define.Deadly))
            {
                hero.Dead();
            }
        }

        public void OnCollisionExit(HeroController hero, Collision2D collision)
        {
            if (collision.gameObject.CompareTag(Define.Platform))
            {
                if (collision.gameObject.name.Contains(Define.Slowing))
                {
                    hero.speed = hero.assignedSpeed;
                }
            }
            
        }
    }
}