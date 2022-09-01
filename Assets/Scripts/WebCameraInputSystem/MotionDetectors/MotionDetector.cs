using UnityEngine.Events;
using System.Linq;
using UnityEngine;
using System;

namespace WebCameraInputSystem.MotionDetectors
{
    public abstract class MotionDetector : MonoBehaviour
    {
        [SerializeField] protected WebCamera _webCamera;
        [SerializeField] private float _minDifference = 0.2f;
        [SerializeField] private Renderer _forDebugOrNull;
        [SerializeField, ReadOnly] private float _difference = 0f;
        private float[] _background;

        public event UnityAction OnMotionDetected;

        private void OnEnable()
        {
            //_targetZone = GetZone();
            //_webCamera.Subscribe(_targetZone, OnNewFrame);
            _webCamera.OnNewFrame += OnNewFrame;
        }

        private void OnDisable()
        {
            //_webCamera.UnSubscribe(OnNewFrame);
            _webCamera.OnNewFrame -= OnNewFrame;
        }

        protected abstract RectInt GetZone();

        private void OnNewFrame(Texture2D motionTexture)
        {
            var targetZone = GetZone();
            var pixels = GetRect(motionTexture, targetZone);
            var grayscaled = GrayScalePixels(pixels);

            _difference = CalcDifference(grayscaled, _background);

            if (_difference > _minDifference)
                OnMotionDetected?.Invoke();

            UpdateBackground(grayscaled);

            Texture2D zoneTexture = new Texture2D(targetZone.width, targetZone.height);
            zoneTexture.SetPixels(pixels);
            zoneTexture.Apply();
            if (_forDebugOrNull != null)
                _forDebugOrNull.material.mainTexture = zoneTexture;
        }

        private float[] GrayScalePixels(Color[] pixels)
        {
            return pixels.Select(x => x.grayscale).ToArray();
        }

        private float CalcDifference(float[] pixels, float[] background)
        {
            if (background == null) return 0;

            var difference = 0f;
            for (var i = 0; i < background.Length; i++)
                difference += MathF.Abs(MathF.Abs(background[i]) - MathF.Abs(pixels[i]));
            difference /= background.Length;
            return difference;
        }

        private void UpdateBackground(float[] pixels)
        {
            if (_background == null)
            {
                _background = pixels;
                return;
            }
            for (int i = 0; i < _background.Length; i++)
                _background[i] = (_background[i] + pixels[i]) / 2;
        }

        private Color[] GetRect(Texture2D motionTexture, RectInt rect)
        {
            return motionTexture.GetPixels(rect.x, rect.y, rect.width, rect.height);
        }
    }
}
