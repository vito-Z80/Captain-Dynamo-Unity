using Game.Platforms;
using Game.Platforms.ForEditor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ConveyorBelt))]
    public class BeltRepeatEditor : UnityEditor.Editor
    {
        private RepeatPlatform _repeatPlatform;

        private void OnEnable()
        {
            _repeatPlatform = target as RepeatPlatform;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUI.changed)
            {
                _repeatPlatform.UpdateInEditor();
            }
        }
    }
    /////////////////////////////////////////////////////////////
    
    [CustomEditor(typeof(FixedPlatform))]
    public class FixedPlatformRepeatEditor : UnityEditor.Editor
    {
        private RepeatPlatform _repeatPlatform;

        private void OnEnable()
        {
            _repeatPlatform = target as RepeatPlatform;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUI.changed)
            {
                _repeatPlatform.UpdateInEditor();
            }
        }
    }
    
    /////////////////////////////////////////////////////////////
    
    [CustomEditor(typeof(FixedPlatformWithoutSides))]
    public class FixedPlatformWithoutSidesRepeatEditor : UnityEditor.Editor
    {
        private RepeatPlatformWithoutSides _repeatPlatform;

        private void OnEnable()
        {
            _repeatPlatform = target as RepeatPlatformWithoutSides;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUI.changed)
            {
                _repeatPlatform.UpdateInEditor();
            }
        }
    }
}