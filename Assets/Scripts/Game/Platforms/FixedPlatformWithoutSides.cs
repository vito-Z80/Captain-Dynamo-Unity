using Game.Platforms.ForEditor;

namespace Game.Platforms
{
    public class FixedPlatformWithoutSides : RepeatPlatformWithoutSides, IPlatform
    {
        private void Start()
        {
            UpdateInEditor();
        }


        public PlatformType GetPlatformType()
        {
            return platformType;
        }
    }
}

