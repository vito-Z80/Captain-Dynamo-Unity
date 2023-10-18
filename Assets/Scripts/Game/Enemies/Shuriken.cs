using Animations;
using Game.Actions;
using UnityEngine;

namespace Game.Enemies
{
    public class Shuriken : FixedWalkAction
    {
        private AnimationSprite _animationSprite;


        private void Awake()
        {
            step = Vector3.right / 2.0f;
            _animationSprite = GetComponent<AnimationSprite>();
        }

        private void FixedUpdate()
        {
            Move();
            FlipX(_animationSprite);
        }
    }
}