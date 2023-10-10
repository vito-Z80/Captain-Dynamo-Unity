using Game.Platforms.ForEditor;

namespace Game.Platforms
{
    
    
    public class FixedPlatform : RepeatPlatform, IPlatform
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