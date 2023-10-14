using Animations;
using Game.Actions;
using Game.Enemies;
using Game.Platforms.ForEditor;
using UnityEngine;
using AnimationState = Animations.AnimationState;

namespace Game.Platforms
{
    
    public class ConveyorBelt : RepeatPlatform, IPlatform
    {
        [SerializeField] public float resistance = 0.75f;
        
        private void Start()
        {
            UpdateInEditor();
            foreach (var o in Part)
            {
                o.GetComponent<AnimationSprite>().SetState(AnimationState.Idle);
            }
        }

        public PlatformType GetPlatformType()
        {
            return platformType;
        }
        private void OnCollisionEnter2D(Collision2D walker)
        {
            var movable = walker.gameObject.GetComponent<IMovable>();
            if (movable is null) return;
            var dir = platformType == PlatformType.ConveyorBeltLeft ? Vector2.left : Vector2.right;
            movable.SetAdditionalSpeed(resistance * dir);
        }
        private void OnCollisionExit2D(Collision2D walker)
        {
            var movable = walker.gameObject.GetComponent<IMovable>();
            if (movable is null) return;
            movable.SetAdditionalSpeed(Vector2.zero);
        }
    }
}