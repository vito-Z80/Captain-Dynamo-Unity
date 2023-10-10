using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Behaviors
{
    public class AutoWalk : MonoBehaviour
    {
        public HorizontalDirection GetHorizontalDirection(TileBase[] tiles, string compareName)
        {
            if (tiles[0] != null)
            {
                if (tiles[0].name.Contains(compareName, StringComparison.OrdinalIgnoreCase))
                {
                    return HorizontalDirection.Right;
                }
            }

            if (tiles[1] != null)
            {
                if (tiles[1].name.Contains(compareName, StringComparison.OrdinalIgnoreCase))
                {
                    return HorizontalDirection.Left;
                }
            }

            if (tiles[2] != null)
            {
                if (tiles[2].name.Contains(compareName, StringComparison.OrdinalIgnoreCase))
                {
                    return HorizontalDirection.Right;
                }
            }

            if (tiles[3] != null)
            {
                if (tiles[3].name.Contains(compareName, StringComparison.OrdinalIgnoreCase))
                {
                    return HorizontalDirection.Left;
                }
            }

            return HorizontalDirection.Stay;
        }
    }
}