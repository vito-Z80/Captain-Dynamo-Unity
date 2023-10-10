using UnityEngine;

namespace Gizmo
{
    public class RespawnEditor : MonoBehaviour
    {
        private readonly Vector3 _size = new Vector3(16.0f, 16.0f, 0.0f);

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.4f);
            Gizmos.DrawCube(transform.position, _size);
        }
    }
}