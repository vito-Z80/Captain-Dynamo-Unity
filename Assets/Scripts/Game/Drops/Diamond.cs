using UnityEngine;

namespace Game.Drops
{
    public class Diamond : MonoBehaviour, ICollected
    {
        private ScoreDisplay _scoreDisplay;
        private ItemPoints _itemPoints;
        private BoxCollider2D _boxCollider2D;
        private SpriteRenderer _spriteRenderer;
        public GameData gameData;

        private void Awake()
        {
            _scoreDisplay = GetComponent<ScoreDisplay>();
            _itemPoints = GetComponent<ItemPoints>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Collect()
        {
            _boxCollider2D.enabled = false;
            _spriteRenderer.enabled = false;
            _scoreDisplay.Show();
            gameData.CollectScores(_itemPoints.itemScores);
            if (name.Contains(Define.Diamond)) gameData.CollectDiamond();
        }
    }
}