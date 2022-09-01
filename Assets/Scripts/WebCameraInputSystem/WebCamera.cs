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
        [SerializeField] private Renderer _cameraOutput;
        [SerializeField] private Renderer _motionFrameOutput;
        private WebCamTexture _webcamTexture;
        private float _prevTime = 0;

        public Vector2Int MotionDetectFrameSize => _motionDetectFrameSize;

        public event UnityAction<WebCamera, Texture2D> OnNewFrame;

        private void Awake()
        {
            var readers = FindObjectsOfType<WebCamera>();
            if (readers.Length > 1)
                Debug.LogWarning("It is recommended to use only one Web Camera Reader on scene");

            _webcamTexture = new WebCamTexture(_requestedFrameSize.x, _requestedFrameSize.y, _requestedFps);
            if (_cameraOutput != null)
                _cameraOutput.material.mainTexture = _webcamTexture;
        }

        [Obsolete]
        private void OnEnable()
        {
            _webcamTexture.Play();
            _prevTime = Time.timeSinceLevelLoad;
        }

        private void OnDisable()
        {
            _webcamTexture.Stop();
        }

        [Obsolete]
        private void Update()
        {
            if (!_webcamTexture.isPlaying) return;
            if (!_webcamTexture.didUpdateThisFrame) return;
            if (Time.timeSinceLevelLoad - _prevTime < 1f / _detectionFps) return;
            _prevTime = Time.timeSinceLevelLoad;

            var motionTexture = new Texture2D(_webcamTexture.width, _webcamTexture.height);
            motionTexture.SetPixels(_webcamTexture.GetPixels());
            TextureScaler.Scale(
                motionTexture,
                _motionDetectFrameSize.x,
                _motionDetectFrameSize.y,
                FilterMode.Point);
            OnNewFrame?.Invoke(this, motionTexture);

            if (_motionFrameOutput)
                _motionFrameOutput.material.mainTexture = motionTexture;
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
