using UnityEngine;

namespace Game.Platforms
{
    public class Platform : MonoBehaviour, IPlatform
    {
        [SerializeField] public PlatformType platformType;

        public PlatformType GetPlatformType()
        {
            return platformType;
        }
    }
}