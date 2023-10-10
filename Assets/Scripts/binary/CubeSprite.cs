using System.Collections.Generic;
using UnityEngine;

namespace binary
{
    
    
    struct Pixel
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public GameObject cube;
    }
    
    
    public class CubeSprite
    {
        private Pixel pix = new Pixel();

        
        public void asds()
        {
            pix.position = Vector3.down;
        }
        

    }
}