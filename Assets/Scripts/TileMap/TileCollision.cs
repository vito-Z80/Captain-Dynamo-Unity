using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMap
{
    /*
     * Цепляется к TileMap для чекания тайлов.
     */
    public class TileCollision : MonoBehaviour
    {
        protected TileBase CenterLeftTile(Tilemap tilemap, Bounds bounds)
        {
            var point = tilemap.WorldToCell(
                new Vector3(
                    bounds.min.x,
                    bounds.min.y + bounds.extents.y,
                    0.0f
                ));
            return tilemap.GetTile(point);
        }

        protected TileBase CenterRightTile(Tilemap tilemap, Bounds bounds)
        {
            var point = tilemap.WorldToCell(
                new Vector3(
                    bounds.max.x,
                    bounds.min.y + bounds.extents.y,
                    0.0f
                ));
            return tilemap.GetTile(point);
        }

        protected TileBase CenterBottomTile(Tilemap tilemap, Bounds bounds)
        {
            var point = tilemap.WorldToCell(
                new Vector3(
                    bounds.center.x,
                    bounds.min.y - 1.0f,
                    0.0f
                ));
            return tilemap.GetTile(point);
        }


        [CanBeNull]
        public TileBase GetLeftTopTile(Tilemap tilemap, Bounds bounds)
        {
            var point = tilemap.WorldToCell(new Vector3(
                bounds.center.x - bounds.extents.x,
                bounds.center.y + bounds.extents.y
            ));
            return tilemap.GetTile(point);
        }

        [CanBeNull]
        protected TileBase RightBottomRightTile(Tilemap tilemap, Bounds bounds)
        {
            var point = tilemap.WorldToCell(new Vector3(
                bounds.center.x + bounds.size.x,
                bounds.center.y
            ) + Vector3.right);
            return tilemap.GetTile(point);
        }

        protected TileBase RightBottomBottomTile(Tilemap tilemap, Bounds bounds)
        {
            var point = tilemap.WorldToCell(new Vector3(
                bounds.center.x + bounds.size.x,
                bounds.center.y
            ) + Vector3.down);
            return tilemap.GetTile(point);
        }

        [CanBeNull]
        public TileBase LeftBottomLeftTile(Tilemap tilemap, Bounds bounds)
        {
            var point = tilemap.WorldToCell(bounds.center + Vector3.left);
            return tilemap.GetTile(point);
        }

        public TileBase LeftBottomBottomTile(Tilemap tilemap, Bounds bounds)
        {
            var point = tilemap.WorldToCell(bounds.center + Vector3.down);
            return tilemap.GetTile(point);
        }

        public Vector3Int GetLeftBottomTilePos(Tilemap tilemap, Bounds bounds)
        {
            var point = tilemap.WorldToCell(bounds.center);
            return point;
        }

        [CanBeNull]
        public TileBase GetRightTopTile(Tilemap tilemap, Bounds bounds)
        {
            var point = tilemap.WorldToCell(bounds.max + bounds.extents);
            return tilemap.GetTile(point);
        }


        public void GetAllTiles(Tilemap tilemap, Bounds bounds, TileBase[] array)
        {
            array[0] = GetLeftTopTile(tilemap, bounds);
            array[1] = GetRightTopTile(tilemap, bounds);
            array[2] = LeftBottomLeftTile(tilemap, bounds);
            array[3] = RightBottomRightTile(tilemap, bounds);
        }

        // public bool IntersectBottom(BoxCollider2D collider, string tileNameTag)
        // {
        //     var bLeft = GetLeftBottomTile(collider.bounds);
        //     var bRight = GetRightBottomTile(collider.bounds);
        //     return bLeft != null &&
        //            (bLeft.name.Contains(tileNameTag) || bRight != null && bRight.name.Contains(tileNameTag));
        // }
        
    }
}