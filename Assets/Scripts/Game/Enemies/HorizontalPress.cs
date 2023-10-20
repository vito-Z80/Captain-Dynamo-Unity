using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemies
{
    public class HorizontalPress : MonoBehaviour
    {
        public bool isPressLeft;


        [SerializeField] private float waitTime;
        [SerializeField] private float startTime;

        public Sprite leftPressSprite;
        public Sprite rightPressSprite;

        public GameObject shaft;
        public GameObject shaftMask;

        private SpriteRenderer _pressSpriteRenderer;
        private HorizontalDirection _direction = HorizontalDirection.Stay;
        private BoxCollider2D _boxCollider2D;

        private const float ImpactForce = 5.0f;
        private const float RecoilForce = 1.0f;

        private Vector3 _idlePosition;

        private bool isGoesBack = false;

        private void Start()
        {
            _idlePosition = transform.position;
            _pressSpriteRenderer = GetComponent<SpriteRenderer>();
            _pressSpriteRenderer.sprite = isPressLeft ? leftPressSprite : rightPressSprite;
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _boxCollider2D.offset = new Vector2(isPressLeft ? 6.0f : -6.0f, 0.0f);
            _boxCollider2D.enabled = false;
            var position = shaftMask.transform.position;
            var shaftWidth = shaftMask.GetComponent<SpriteMask>().sprite.bounds.extents.x;
            var pressWidth = GetComponent<SpriteRenderer>().sprite.bounds.extents.x;

            shaftMask.transform.position = new Vector3(
                isPressLeft
                    ? transform.position.x + (shaftWidth - pressWidth)
                    : transform.position.x - (shaftWidth - pressWidth),
                position.y,
                position.z
            );
            position = shaft.transform.position;
            shaft.transform.position = new Vector3(
                isPressLeft
                    ? transform.position.x - (shaftWidth + pressWidth)
                    : transform.position.x + (shaftWidth + pressWidth),
                position.y,
                position.z
            );
            shaftMask.SetActive(true);
        }


        private void FixedUpdate()
        {
            switch (_direction)
            {
                case HorizontalDirection.Left:
                    MoveLeft();
                    break;
                case HorizontalDirection.Right:
                    MoveRight();
                    break;
                case HorizontalDirection.Stay:
                    Wait();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Wait()
        {
            startTime += Time.deltaTime;
            if (startTime < waitTime) return;
            startTime = 0.0f;
            _direction = isPressLeft ? HorizontalDirection.Right : HorizontalDirection.Left;
            _boxCollider2D.enabled = true;
        }

        private void MoveRight()
        {
            var force = isPressLeft ? ImpactForce : RecoilForce;
            Move(force, Vector3.right);
        }

        private void MoveLeft()
        {
            var force = !isPressLeft ? ImpactForce : RecoilForce;
            Move(force, Vector3.left);
        }

        private void Move(float force, Vector3 direction)
        {
            transform.position += direction * force;
            shaft.transform.position += direction * force;


            if (isGoesBack)
            {
                if (direction == Vector3.left && transform.position.x <= _idlePosition.x)
                {
                    transform.position = _idlePosition;
                    _direction = HorizontalDirection.Stay;
                    startTime = 0.0f;
                    isGoesBack = false;
                    _boxCollider2D.enabled = false;
                }

                if (direction == Vector3.right && transform.position.x >= _idlePosition.x)
                {
                    transform.position = _idlePosition;
                    _direction = HorizontalDirection.Stay;
                    startTime = 0.0f;
                    isGoesBack = false;
                    _boxCollider2D.enabled = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Define.Ground) || other.CompareTag(Define.Deadly))
            {
                _direction = _direction == HorizontalDirection.Left
                    ? HorizontalDirection.Right
                    : HorizontalDirection.Left;
                isGoesBack = true;
            }
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            GetComponent<SpriteRenderer>().sprite = isPressLeft ? leftPressSprite : rightPressSprite;
        }
#endif
    }
}