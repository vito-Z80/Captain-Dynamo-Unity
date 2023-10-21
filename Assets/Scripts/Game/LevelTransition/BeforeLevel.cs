using UnityEngine;

namespace Game.LevelTransition
{
    public class BeforeLevel : MonoBehaviour
    {
        public GameData gameData;
        public FingerCounter fingerCounter;
        public TextMesh currentCavern;

        [HideInInspector] public bool isDone = false;

        private void Start()
        {
            currentCavern.text = gameData.currentLevel.ToString();
            StartCoroutine(fingerCounter.Counter(() => { isDone = true; }));
        }
    }
}