using Animations;
using UnityEngine;

namespace Game.Actions
{
    public class FixedWalkAction : MonoBehaviour
    {
        public float offset;
        private Vector3 _leftX = Vector3.zero;
        private Vector3 _rightX = Vector3.zero;
        [HideInInspector] public float direction;
        [HideInInspector]public Vector3 step = Vector3.right / 3.0f;

        private void Start()
        {
            direction =  1.0f;
            var pos = transform.position;
            if (offset > 0.0f)
            {
                _rightX = pos + Vector3.right * offset;
                _leftX = pos;
            }
            else
            {
                _leftX = pos + Vector3.right * offset;
                _rightX = pos;
            }
        }

        protected void Move()
        {
            if (transform.position.x <= _leftX.x)
            {
                direction = 1.0f;
            }

            if (transform.position.x >= _rightX.x)
            {
                direction = -1.0f;
            }

            transform.position += step * direction;
        }

        protected void FlipX(AnimationSprite animationSprite)
        {
            animationSprite.FlipX(direction > 0.0f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var pos = transform.position + Vector3.down * 8f;
            var toPos = transform.position + new Vector3(offset, -8.0f, 0.0f);
            Gizmos.DrawLine(pos, toPos);
        }
    }
}