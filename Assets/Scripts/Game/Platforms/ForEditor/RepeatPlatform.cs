using System.Collections.Generic;
using System.Linq;
using Animations;
using UnityEngine;

namespace Game.Platforms.ForEditor
{
    public class RepeatPlatform : MonoBehaviour
    {
        public GameObject leftImage;
        public GameObject centralImage;
        public GameObject rightImage;

        public bool leftImageVisible = true;
        public bool rightImageVisible = true;

        [SerializeField] public PlatformType platformType;
        [SerializeField] [Range(0, 10)] public int repeatCount = 0;
        [SerializeField] public float spriteWidth = 0.0f;
        [SerializeField] public float spriteHeight = 0.0f;
        protected readonly List<GameObject> Part = new List<GameObject>();
        [HideInInspector] public BoxCollider2D boxCollider2D;

        public void UpdateInEditor()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            var children = transform.Cast<Transform>().ToList();
            foreach (var child in children)
            {
                DestroyImmediate(child.gameObject);
            }

            Part.Clear();
            //
            var fullWidth = spriteWidth * (repeatCount + 2);
            var posX = -fullWidth / 2.0f + spriteWidth / 2f;

            var leftObj = GetInstantiate(ref posX, leftImage.gameObject);
            Part.Add(leftObj);

            for (var i = 0; i < repeatCount; i++)
            {
                var centerObj = GetInstantiate(ref posX, centralImage.gameObject);
                Part.Add(centerObj);
            }

            var rightObj = GetInstantiate(ref posX, rightImage.gameObject);
            Part.Add(rightObj);

            //  box collider create
            // _bc.offset = Vector2.zero;
            boxCollider2D.size = new Vector2((repeatCount + 2) * spriteWidth, spriteHeight);
            Part[0].SetActive(leftImageVisible);
            Part[^1].SetActive(rightImageVisible);
        }

        private GameObject GetInstantiate(ref float posX, GameObject pref)
        {
            var pos = transform.position + new Vector3(posX, 0, 0);
            var obj = Instantiate(pref, pos, Quaternion.identity, transform);
            var aSpr = obj.GetComponent<AnimationSprite>();
            if (aSpr is not null)
            {
                foreach (var asp in aSpr.animations)
                {
                    asp.isPlayBackward = platformType == PlatformType.ConveyorBeltRight;
                }
            }

            posX += spriteWidth;
            return obj;
        }
    }
}