using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace WebCameraInputSystem
{
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private Vector2Int _requestedFrameSize=new Vector2Int(426, 240);
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

        private void OnEnable()
        {
            _webcamTexture.Play();
            StartCoroutine(Ticking());
        }

        private void OnDisable()
        {
            _webcamTexture.Stop();
        }

        public void Subscribe(RectInt rect, UnityAction<Color[]> action)
        {
            //_subscribers.Add(new TextureRectSubscriber(rect, action));
        }

        public void UnSubscribe(UnityAction<Color[]> action)
        {
            //_subscribers.RemoveAll(x=>x.Action == action);
        }

        [Obsolete]
        private IEnumerator Ticking()
        {
            while (enabled)
            {
                yield return new WaitForSecondsRealtime(1 / _requestedFps);

                Texture2D motionTexture = new Texture2D(_webcamTexture.width, _webcamTexture.height);
                motionTexture.SetPixels(_webcamTexture.GetPixels());
                TextureScaler.scale(motionTexture, _motionDetectFrameSize.x, _motionDetectFrameSize.y, FilterMode.Point);

                //var pixels = motionTexture.GetPixels();

                OnNewFrame?.Invoke(motionTexture);
                //foreach (var subscriber in _subscribers.ToList())
                //{
                //    var colors = GetRect(motionTexture, subscriber.TargetRect);
                //    //subscriber.Action.BeginInvoke(colors, null, null);
                //    subscriber.Action.Invoke(colors);
                //}
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
