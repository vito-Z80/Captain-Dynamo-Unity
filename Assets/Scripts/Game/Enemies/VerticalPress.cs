using Camera;
using UnityEngine;

namespace Game.Enemies
{
    public class VerticalPress : MonoBehaviour
    {
        public float gravityScale;
        public float climbForce;

        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider2D;
        private Vector3 _topPosition;

        private float _startTime;
        
        private UnityEngine.Camera _camera;
        private CameraController _cameraController;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _rigidbody2D.gravityScale = gravityScale;
            _rigidbody2D.sleepMode = RigidbodySleepMode2D.StartAsleep;
            _topPosition = transform.position;
            _startTime = Random.value * 2.0f;
            _camera = UnityEngine.Camera.main;
            _cameraController = _camera.GetComponent<CameraController>();
        }


        private void FixedUpdate()
        {
            _startTime -= Time.deltaTime;
            if (_startTime >= 0.0f) return;
            if (_rigidbody2D.IsSleeping())
            {
                _rigidbody2D.WakeUp();
            }
            if (_rigidbody2D.gravityScale > 0.0f) return;
            if (transform.position.y < _topPosition.y) return;
            _rigidbody2D.gravityScale = gravityScale;
            _rigidbody2D.velocity = Vector2.zero;
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            var rect = new Rect(
                _boxCollider2D.bounds.min,
                _boxCollider2D.bounds.size
            );
            _cameraController.CameraShake(rect);
            _rigidbody2D.gravityScale = 0.0f;
            _rigidbody2D.velocity = Vector2.up * (climbForce * 60.0f);
        }
    }
}