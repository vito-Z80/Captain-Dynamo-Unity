using UnityEngine;

namespace Game.Actions
{
    public interface IMovable
    {
        public void SetAdditionalSpeed(Vector2 additionalSpeedFactor);
        public void SetSpeed(float speed);

        public void Jump(float jumpForce);

    }
}