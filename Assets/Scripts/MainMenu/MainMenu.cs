using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {

        public GameData gameData;
        public TextMesh anyKeyLabel;
        private readonly Color[] _flash = new []
        {
            Color.blue, 
            Color.red, 
            Color.magenta, 
            Color.green, 
            Color.cyan, 
            Color.yellow, 
            Color.white, 
        };

        private float _flashTimer;
        private int _flashCount;
        private void FixedUpdate()
        {
            if (Input.anyKey)
            {
                gameData.Reset();
                SceneManager.LoadScene("Scenes/LevelTransition");
            }
            
            if ((_flashTimer += Time.deltaTime) < 0.1f) return;
            _flashTimer = 0.0f;
            anyKeyLabel.color = _flash[_flashCount++];
            if (_flashCount < _flash.Length) return;
            _flashCount = 0;
        }
    }
}