using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Win
{
    public class CongratulationsText : MonoBehaviour
    {
        private const string ConText = "Congratulations";
        private readonly List<UnityEngine.UI.Text> _word = new List<UnityEngine.UI.Text>();
        private readonly List<int> _colorIds = new List<int>();
        private float _letterTimer;
        private float _wordTimer;
        private bool _isLoop = true;
        [HideInInspector] public bool canPressAnyKey = false;

        public UnityEngine.UI.Text letterPref;

        public float speed = 2f;
        public float height = 32f;
        public float between = 1f / 4f;

        private void Start()
        {
            Create();
            StartCoroutine(PrintByLetter());
        }


        private void Update()
        {
            if (canPressAnyKey && Input.anyKeyDown) SceneManager.LoadScene("Scenes/MainMenu");
            _wordTimer += Time.deltaTime * speed;
            _letterTimer = _wordTimer;
            foreach (var l in _word)
            {
                // if (!l.gameObject.activeSelf) continue;
                var rt = l.GetComponent<RectTransform>();
                JumpLetter(rt, Mathf.Sin(_letterTimer) * height);
                _letterTimer -= between;
            }
        }


        private IEnumerator PrintByLetter()
        {
            yield return new WaitForSeconds(1.5f);
            foreach (var l in _word)
            {
                yield return new WaitForSeconds(0.2f);
                yield return null;
                l.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(1.0f);
            StartCoroutine(Flash());
            yield return new WaitForSeconds(3.0f);
            canPressAnyKey = true;
            yield return null;
        }

        private IEnumerator Flash()
        {
            while (_isLoop)
            {
                yield return new WaitForSeconds(1f / 25.0f);
                for (var id = 0; id < _word.Count; id++)
                {
                    if (--_colorIds[id] < 0) _colorIds[id] = Define.colors.Length - 1;
                    _word[id].color = Define.colors[_colorIds[id]];
                }
            }

            yield return null;
        }


        private void JumpLetter(RectTransform rt, float y)
        {
            var pos = rt.anchoredPosition;
            pos.y = Mathf.Abs(y);
            rt.anchoredPosition = pos;
        }


        private void Create()
        {
            var position = Vector3.left * 112f;
            foreach (var c in ConText)
            {
                var letter = GetLetter(c);
                var rc = letter.GetComponent<RectTransform>();
                rc.anchoredPosition = position;
                var size = rc.sizeDelta;
                position += Vector3.right * size.x;
                _word.Add(letter);
                letter.gameObject.SetActive(false);
            }

            //
            var colorCount = 0;
            for (var id = 0; id < _word.Count; id++)
            {
                _colorIds.Add(colorCount);
                if (++colorCount >= Define.colors.Length) colorCount = 0;
            }
        }

        private UnityEngine.UI.Text GetLetter(char letter)
        {
            var l = Instantiate(letterPref, gameObject.transform, true);
            l.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
            l.text = letter.ToString();
            return l;
        }

        private void OnDisable()
        {
            _isLoop = false;
            foreach (var l in _word)
            {
                Destroy(l);
            }

            _word.Clear();
        }
    }
}