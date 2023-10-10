using UnityEngine;

namespace Game.Platforms
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Vector3 min = Vector3.zero;
        [SerializeField] private Vector3 max = Vector3.zero;
        [SerializeField] private float speed;
        private PlatformMoveArea _platformMoveArea;

        private void Start()
        {
            _platformMoveArea = new PlatformMoveArea(transform, min, max, speed);
        }

        private void Update()
        {
            _platformMoveArea.Move();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(min + transform.position, max + transform.position);
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            other?.collider.transform.SetParent(transform);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.activeInHierarchy)
            {
                other.collider.transform.SetParent(null);
            }
        }
    }
}