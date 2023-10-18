using UnityEngine;

namespace Game.Enemies
{
    public class VerticalPress : MonoBehaviour
    {
        public float gravityScale;
        public float climbForce;

        private Rigidbody2D _rigidbody2D;
        private Vector3 _topPosition;


        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.gravityScale = gravityScale;
            _topPosition = transform.position;
        }


        private void FixedUpdate()
        {
            if (_rigidbody2D.gravityScale > 0.0f) return;
            if (transform.position.y < _topPosition.y) return;
            _rigidbody2D.gravityScale = gravityScale;
            _rigidbody2D.velocity = Vector2.zero;
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            _rigidbody2D.gravityScale = 0.0f;
            _rigidbody2D.velocity = Vector2.up * (climbForce * 60.0f);
        }
    }
}