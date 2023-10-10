using System.Collections;
using UnityEngine;

namespace Game.Enemies
{
    public class Electricity : MonoBehaviour
    {
        public float workTime;
        public float waitTime;

        public ParticleSystem particleSystemLeft;
        public ParticleSystem particleSystemRight;
        private BoxCollider2D _boxCollider2D;

        private float _timer;
        private int _state = 1;
        private bool _corExe = false;

        private void Start()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }


        private void FixedUpdate()
        {
            if (_corExe) return;
            if (_timer >= 0.0f)
            {
                _timer -= Time.deltaTime;
                return;
            }

            _corExe = true;
            StartCoroutine(SetState());
        }


        private IEnumerator SetState()
        {
            if ((_state & 1) == 1)
            {
                Disable();
                _timer = waitTime;
            }
            else
            {
                Enable();
                _timer = workTime;
                yield return new WaitForSeconds(0.5f);
                _boxCollider2D.enabled = true;
            }

            _state ^= 1;
            yield return null;
            _corExe = false;
        }

        private void Enable()
        {
            particleSystemLeft.Play();
            particleSystemRight.Play();
        }

        private void Disable()
        {
            particleSystemLeft.Stop();
            particleSystemRight.Stop();
            _boxCollider2D.enabled = false;
        }
    }
}