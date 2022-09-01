using WebCameraInputSystem.Utils;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace WebCameraInputSystem
{
    [AddComponentMenu("WebCameraInputSystem/WebCamera")]
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private Vector2Int _requestedFrameSize = new Vector2Int(1920, 1080);
        [SerializeField] private int _requestedFps = 30;
        [SerializeField] private Vector2Int _motionDetectFrameSize = new Vector2Int(192, 108);
        [SerializeField] private int _detectionFps = 10;
        private WebCamTexture _webCamTexture;
        private float _prevTime = 0;

        public WebCamTexture WebCamTexture => _webCamTexture;
        public Vector2Int MotionDetectFrameSize => _motionDetectFrameSize;

        public event UnityAction<WebCamera, Texture2D> OnNewFrame;

        private void Awake()
        {
            var cams = FindObjectsOfType<WebCamera>();
            if (cams.Length > 1)
                Debug.LogWarning("It is recommended to use only one Web Camera Reader on scene");

            _webCamTexture = new WebCamTexture(_requestedFrameSize.x, _requestedFrameSize.y, _requestedFps);
        }

        [Obsolete]
        private void OnEnable()
        {
            _webCamTexture.Play();
            _prevTime = Time.timeSinceLevelLoad;
        }

        private void OnDisable()
        {
            _webCamTexture.Stop();
        }

        [Obsolete]
        private void Update()
        {
            if (!_webCamTexture.isPlaying) return;
            if (!_webCamTexture.didUpdateThisFrame) return;
            if (Time.timeSinceLevelLoad - _prevTime < 1f / _detectionFps) return;
            _prevTime = Time.timeSinceLevelLoad;

            var motionTexture = new Texture2D(_webCamTexture.width, _webCamTexture.height);
            motionTexture.SetPixels(_webCamTexture.GetPixels());
            TextureScaler.Scale(
                motionTexture,
                _motionDetectFrameSize.x,
                _motionDetectFrameSize.y,
                FilterMode.Point);
            OnNewFrame?.Invoke(this, motionTexture);
        }

        private class TextureRectSubscriber
        {
            public UnityAction<Color[]> Action { get; }
            public RectInt TargetRect { get; }

            public TextureRectSubscriber(RectInt targetRect, UnityAction<Color[]> action)
            {
                TargetRect = targetRect;
                Action = action;
            }
        }
    }
}
