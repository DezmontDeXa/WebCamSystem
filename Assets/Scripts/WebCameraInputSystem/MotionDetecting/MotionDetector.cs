using WebCameraInputSystem.ZoneGetters;
using WebCameraInputSystem.Utils;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace WebCameraInputSystem.MotionDetecting
{
    [AddComponentMenu("WebCameraInputSystem/MotionDetector")]
    public class MotionDetector : MonoBehaviour
    {
        [SerializeField] protected WebCamera _webCamera;
        [SerializeField] private float _minDifference = 0.05f;
        [SerializeField] private ZoneGetter _zoneGetter;
        [SerializeField, ReadOnly] private float _difference = 0f;
        [SerializeField] private DetectMode _detectMode;
        private float[] _background;

        public bool HasMotion => _difference > _minDifference;

        public float Difference => _difference;

        public ZoneGetter ZoneGetter => _zoneGetter;

        public event UnityAction<MotionDetector, float> OnFrameProcessed;

        public event UnityAction<MotionDetector, float> OnMotionDetected;

        private void OnEnable()
        {
            _webCamera.OnNewFrame += OnNewFrame;
        }

        private void OnDisable()
        {
            _webCamera.OnNewFrame -= OnNewFrame;
        }

        private void OnNewFrame(WebCameraFrame frame)
        {
            var targetZone = _zoneGetter.GetZone(frame.MotionTextureSize);
            var bytes = frame.MotionTexture.GetRawTextureData();
            var bytesOfZone = Alg.CropByBytes(bytes, frame.MotionTextureSize, targetZone);
            var grayscaled = Alg.GetGrayScale(bytesOfZone);

            var prevFrameHasMotion = HasMotion;
            _difference = Alg.CalcDifference(grayscaled, _background);
            InvokeIfNeeded(prevFrameHasMotion, _difference, _minDifference, _detectMode);
            UpdateBackground(grayscaled);
            OnFrameProcessed?.Invoke(this, _difference);
        }

        private void InvokeIfNeeded(bool prevFrameHasMotion, float difference, float minDifference, DetectMode detectMode)
        {
            if (difference < minDifference) return;
            if (!prevFrameHasMotion)
                OnMotionDetected?.Invoke(this, difference);
            else
            if (detectMode == DetectMode.Continious)
                OnMotionDetected?.Invoke(this, difference);
        }

        private void UpdateBackground(float[] pixels)
        {
            if (_background == null)
            {
                _background = pixels;
                return;
            }
            if (_background.Length != pixels.Length) return;

            for (var i = 0; i < _background.Length; i++)
                //_background[i] = MathF.Round(Alg.SquareMediateValue(_background[i], pixels[i]), 3);
                _background[i] = MathF.Round(Alg.MediateValue(_background[i], pixels[i]), 3);
        }

    }
}
