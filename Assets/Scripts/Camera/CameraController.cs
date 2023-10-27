using UnityEngine;
using UnityEngine.U2D;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        public float z;
        [SerializeField] public Transform trackingPoint;
        [SerializeField] public bool horizontal;
        [SerializeField] public bool vertical;
        [HideInInspector] public bool isBlocked;
        private Vector3 _point = Vector3.back * 20f;
        private Vector3 _cameraOffset;

        public AudioSource shakeSound;

        [HideInInspector] public PixelPerfectCamera ppc;

        private int _offsetX;
        private int _screenNumber = 0;
        
        private float _shakeTime;
        private bool _canShake;

        private void Start()
        {
            isBlocked = false;
            ppc = GetComponent<PixelPerfectCamera>();
            var screenSize = Screen.safeArea.size;
            _offsetX = ppc.refResolutionX / 2;
        }


        private void Update()
        {
            if (!isBlocked)
            {
                CalculateScreen();
                if (vertical) _point.y = trackingPoint.position.y;
                if (horizontal) _point.x = trackingPoint.position.x;
                transform.position = Vector3.Lerp(transform.position, _point + _cameraOffset, Time.deltaTime * 4.0f);
            }

            var pos = new Vector3(
                Mathf.Round(transform.position.x),
                Mathf.Round(transform.position.y),
                z
            );
        }

        private void FixedUpdate()
        {
            if (_canShake) Shake();
        }

        private Rect GetCameraRect()
        {
            return new Rect(
                ppc.transform.position.x - ppc.refResolutionX / 2.0f,
                ppc.transform.position.y - ppc.refResolutionY / 2.0f,
                ppc.refResolutionX,
                ppc.refResolutionY
            );
        }

        private void CalculateScreen()
        {
            if (trackingPoint.position.x + _offsetX < 0.0f)
            {
                _screenNumber = (int)((trackingPoint.position.x + _offsetX) / ppc.refResolutionX) - 1;
            }
            else
            {
                _screenNumber = (int)(trackingPoint.position.x + _offsetX) / ppc.refResolutionX;
            }

            if (_screenNumber == 0)
            {
                vertical = true;
                horizontal = false;
                _point.x = 0.0f;
            }
            else
            {
                _point.x = _screenNumber * ppc.refResolutionX;
                vertical = false;
                horizontal = true;
            }
        }

        public void CameraShake(Rect srcRect)
        {
            var rect = GetCameraRect();
            if (!rect.Overlaps(srcRect)) return;
            shakeSound.Play();
            _canShake = true;
        }

        private void Shake()
        {
            if (!_canShake && _shakeTime == 0.0f) return;
            _cameraOffset = Random.insideUnitCircle * 16.0f;
            _shakeTime += Time.unscaledDeltaTime;
            if (_shakeTime <= 0.3f) return;
            _cameraOffset = Vector3.zero;
            _shakeTime = _cameraOffset.x;
            _canShake = false;
        }
    }
}