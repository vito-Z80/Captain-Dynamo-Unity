using UnityEngine;

namespace Gizmo
{
    public class GridEditor : MonoBehaviour
    {
        public Vector2 gridSize = new Vector2(16, 16);
        public Vector2 pos = new Vector2(-504, 0);

        private void OnDrawGizmos()
        {
            Grid grid = GetComponent<Grid>();

            if (grid != null)
            {
                Gizmos.color = Color.blue;

                for (float x = pos.x; x < pos.x + grid.cellSize.x * gridSize.x; x += grid.cellSize.x)
                {
                    for (float y = pos.y; y < pos.y + grid.cellSize.y * gridSize.y; y += grid.cellSize.y)
                    {
                        Gizmos.DrawWireCube(new Vector3(x, y, grid.transform.position.z), grid.cellSize);
                    }
                }
            }
        }
    }
}