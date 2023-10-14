using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        public GameObject ui;

        public float z;
        [SerializeField] public Transform trackingPoint;
        [SerializeField] public bool horizontal;
        [SerializeField] public bool vertical;
        [HideInInspector] public bool isBlocked;
        private Vector3 _point = Vector3.back * 20f;
        private Rect _visibleRect = Rect.zero;


        [HideInInspector] public PixelPerfectCamera ppc;

        private int _offsetX;
        private int _screenNumber = 0;
        private int _preScreenNumber = 0;

        private void Start()
        {
            isBlocked = false;
            ppc = GetComponent<PixelPerfectCamera>();
            var screenSize = Screen.safeArea.size;
            _offsetX = ppc.refResolutionX / 2;
            _visibleRect = new Rect(-screenSize, screenSize * 2);
        }


        private void Update()
        {


            if (!isBlocked)
            {
                CalculateScreen();
                if (vertical) _point.y = trackingPoint.position.y;
                if (horizontal) _point.x = trackingPoint.position.x;
                transform.position = Vector3.Lerp(transform.position, _point, Time.deltaTime * 4.0f);
            }

            var pos = new Vector3(
                Mathf.Round(transform.position.x),
                Mathf.Round(transform.position.y),
                z
            );
            ui.transform.position = pos;
            
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
    }
}