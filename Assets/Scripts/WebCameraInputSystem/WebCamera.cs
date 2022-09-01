using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace WebCameraInputSystem
{
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private Vector2Int _requestedFrameSize=new Vector2Int(1920, 1080);
        [SerializeField] private int _requestedFps=30;
        [SerializeField] private Renderer _targetRendererOrNull;
        [SerializeField] private Vector2Int _motionDetectFrameSize = new Vector2Int(192, 108);
        private WebCamTexture _webcamTexture;

        public Vector2Int MotionDetectFrameSize => _motionDetectFrameSize;

        public event UnityAction<Texture2D> OnNewFrame;

        private void Awake()
        {
            var readers = FindObjectsOfType<WebCamera>();
            if (readers.Length > 1)
                Debug.LogWarning("It is recommended to use only one Web Camera Reader on scene");

            _webcamTexture = new WebCamTexture(_requestedFrameSize.x, _requestedFrameSize.y, _requestedFps);
            if (_targetRendererOrNull != null)
                _targetRendererOrNull.material.mainTexture = _webcamTexture;
        }

        [Obsolete]
        private void OnEnable()
        {
            _webcamTexture.Play();
            StartCoroutine(Ticking());
        }

        private void OnDisable()
        {
            _webcamTexture.Stop();
        }

        [Obsolete]
        private IEnumerator Ticking()
        {
            while (enabled)
            {
                yield return new WaitForSecondsRealtime(1 / _requestedFps);

                Texture2D motionTexture = new Texture2D(_webcamTexture.width, _webcamTexture.height);
                motionTexture.SetPixels(_webcamTexture.GetPixels());
                TextureScaler.Scale(motionTexture, _motionDetectFrameSize.x, _motionDetectFrameSize.y, FilterMode.Point);
                OnNewFrame?.Invoke(motionTexture);
            }
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
