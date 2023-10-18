using System;
using UnityEngine;

namespace Game.Platforms
{
    /// <summary>
    /// submerges underwater
    /// </summary>
    public class HidingPlatform : MonoBehaviour
    {
        public float diveOffset;
        public float waitTime;
        public float startAfter;
        private float _timer;
        private Vector3 _topPosition;
        private Vector3 _bottomPosition;
        private VerticalDirection _direction;


        private void Start()
        {
            _topPosition = transform.position;
            _bottomPosition = _topPosition + Vector3.down * diveOffset;
            _direction = VerticalDirection.Stay;
        }


        private void FixedUpdate()
        {
            if ((startAfter -= Time.deltaTime) >= 0.0f) return;
            switch (_direction)
            {
                case VerticalDirection.Up:
                    MoveUp();
                    break;
                case VerticalDirection.Down:
                    MoveDown();
                    break;
                case VerticalDirection.Stay:
                    Wait();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Wait()
        {
            _timer += Time.deltaTime;
            if (_timer < waitTime) return;
            _timer = 0.0f;
            _direction = Math.Abs(_topPosition.y - transform.position.y) < 0.02f
                ? VerticalDirection.Down
                : VerticalDirection.Up;
        }

        private void MoveUp()
        {
            transform.position += Vector3.up * 0.3f;
            if (transform.position.y < _topPosition.y) return;
            transform.position = _topPosition;
            _direction = VerticalDirection.Stay;
        }

        private void MoveDown()
        {
            transform.position += Vector3.down * 0.3f;
            if (transform.position.y > _bottomPosition.y) return;
            transform.position = _bottomPosition;
            _direction = VerticalDirection.Stay;
        }
    }
}