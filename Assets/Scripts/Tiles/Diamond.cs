using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    public interface IAnimatedTile
    {
        public void Action(Vector3 position);
    }

    [CreateAssetMenu(fileName = "Diamond", menuName = "2D/Tiles/Diamond")]
    [Serializable]
    public class Diamond : AnimatedTile, IAnimatedTile
    {
        public void Action(Vector3 position)
        {
            Debug.Log(position);
            
        }
    }
}