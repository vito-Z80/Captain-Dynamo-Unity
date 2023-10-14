using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Drops
{
    public class ScoreDisplay : MonoBehaviour
    {
        public Animation scoreAnimation;
        
        // public int points = 1234;

        private GameObject _go;
        private void Start()
        {
            var rootTransform = transform;
            _go = new GameObject("scoreArea")
            {
                transform =
                {
                    parent = rootTransform.root
                }
            };
            _go.SetActive(false);
            scoreAnimation = Instantiate(scoreAnimation, _go.transform);
            // scoreAnimation.GetComponent<TextMesh>().text = points.ToString();
        }

        public void Show()
        {
            StartCoroutine(ShowScoresAnimation());
        }

        private IEnumerator ShowScoresAnimation()
        {
            _go.transform.position = transform.position;
            _go.SetActive(true);
            scoreAnimation.Play();
            while (scoreAnimation.isPlaying)
            {
                yield return null;
            }
            Destroy(_go);
            yield return new WaitForSeconds(2.0f);
            Destroy(gameObject);
        }
    }
}