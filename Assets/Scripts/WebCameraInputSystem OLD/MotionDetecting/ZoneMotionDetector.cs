using WebCameraInputSystemOLD.MotionDetecting;
using WebCameraInputSystemOLD.ZoneGetters;
using WebCameraInputSystemOLD.Utils;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace WebCameraInputSystemOLD.MotionDetection
{
    [AddComponentMenu("WebCamera InputSystem/Motions/Zone Motion Detector")]
    public class ZoneMotionDetector : MonoBehaviour
    {
        [SerializeField] protected WebCamera _webCamera;
        [SerializeField] private float _minDifference = 0.02f;
        [SerializeField, Range(0, 1f)] private float _minPixelDifference = 0.2f;
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

        private void OnNewFrame(Texture frame, Texture2D motionFrame)
        {
            var motionTextureSize = new Vector2Int(motionFrame.width, motionFrame.height);
            var targetZone = _zoneGetter.GetZone(motionTextureSize);
            var bytes = motionFrame.GetRawTextureData();
            var bytesOfZone = AlgOld.CropByBytes(bytes, motionTextureSize, targetZone);
            var grayscaled = AlgOld.GetGrayScale(bytesOfZone);

            var prevFrameHasMotion = HasMotion;
            _difference = AlgOld.CalcDifference(grayscaled, _background, _minPixelDifference);
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
                        _background[i] = MathF.Round(AlgOld.MediateValue(_background[i], pixels[i]), 3);
                        break;
                    case UpdateBackgroundMode.Squart:
                        _background[i] = MathF.Round(AlgOld.SquareMediateValue(_background[i], pixels[i]), 3);
                        break;
                }
            }
        }
    }
}
