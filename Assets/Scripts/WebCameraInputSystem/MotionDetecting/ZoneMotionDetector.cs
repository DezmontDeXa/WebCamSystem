using WebCameraInputSystem.MotionDetecting;
using WebCameraInputSystem.ZoneGetters;
using WebCameraInputSystem.Utils;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace WebCameraInputSystem.MotionDetection
{
    [AddComponentMenu("WebCamera InputSystem/Motions/Zone Motion Detector")]
    public class ZoneMotionDetector : MonoBehaviour
    {
        [SerializeField] protected WebCamera _webCamera;
        [SerializeField] private float _minDifference = 0.02f;
        [SerializeField] private ZoneGetter _zoneGetter;
        [SerializeField] private DetectMode _detectMode;
        [SerializeField] private UpdateBackgroundMode _updateBackgroundMode;
        [SerializeField, ReadOnly] private float _difference = 0f;
        private float[] _background;

        public bool HasMotion => _difference > _minDifference;

        public float Difference => _difference;

        public event UnityAction<ZoneMotionDetector, float> OnFrameProcessed;

        public event UnityAction<ZoneMotionDetector, float> OnMotionDetected;

        private void OnEnable()
        {
            _webCamera.OnNewFrame += OnNewFrame;
        }

        private void OnDisable()
        {
            _webCamera.OnNewFrame -= OnNewFrame;
        }

        private void OnNewFrame(WebCamTexture frame, Texture2D motionFrame)
        {
            var motionTextureSize = new Vector2Int(motionFrame.width, motionFrame.height);
            var targetZone = _zoneGetter.GetZone(motionTextureSize);
            var bytes = motionFrame.GetRawTextureData();
            var bytesOfZone = Alg.CropByBytes(bytes, motionTextureSize, targetZone);
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
            {
                switch (_updateBackgroundMode)
                {
                    case UpdateBackgroundMode.Linear:
                        _background[i] = MathF.Round(Alg.MediateValue(_background[i], pixels[i]), 3);
                        break;
                    case UpdateBackgroundMode.Squart:
                        _background[i] = MathF.Round(Alg.SquareMediateValue(_background[i], pixels[i]), 3);
                        break;
                }
            }
        }
    }
}
