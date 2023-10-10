using UnityEngine;
using UnityEngine.Tilemaps;

namespace Test
{
    public class SimpleCollisions : MonoBehaviour
    {
        [SerializeField] public Tilemap tilemap;
        private BoxCollider2D _boxCollider2D;

        private void Start()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();

            Debug.Log("X: " + (_boxCollider2D.bounds.center.x - _boxCollider2D.bounds.extents.x));
            Debug.Log("Y: " + (_boxCollider2D.bounds.center.y + _boxCollider2D.bounds.extents.y));
        }

        private void Update()
        {
            // _thisBounds.x = _boxCollider2D.bounds.

            var cell = tilemap.WorldToCell(transform.position);
            var bi = new BoundsInt(cell,new Vector3Int(2,2,0));
            // Debug.Log("X: " + (transform.position.x - 8f));
            // Debug.Log("Y: " + (transform.position.y + 8f));


            var tiles = tilemap.GetTilesBlock(bi);
            Debug.Log(tiles.Length);
            foreach (var tile in tiles)
            {
                if (tile is not null)
                {
                    Debug.Log(tile.name);
                }
            }
        }
    }
}