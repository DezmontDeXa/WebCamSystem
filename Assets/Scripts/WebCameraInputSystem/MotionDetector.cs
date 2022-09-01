using WebCameraInputSystem.ZoneGetters;
using WebCameraInputSystem.Utils;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;
using System;

namespace WebCameraInputSystem
{
    [AddComponentMenu("WebCameraInputSystem/MotionDetector")]
    public class MotionDetector : MonoBehaviour
    {
        [SerializeField] protected WebCamera _webCamera;
        [SerializeField] private float _minDifference = 0.2f;
        [SerializeField] private ZoneGetter _zoneGetter;
        [SerializeField, ReadOnly] private float _difference = 0f;
        private float[] _background;

        public bool HasMotion => _difference > _minDifference;

        public float Difference => _difference;

        public event UnityAction OnFrameProcessed;

        public event UnityAction OnMotionDetected;

        private void OnEnable()
        {
            _webCamera.OnNewFrame += OnNewFrame;
        }

        private void OnDisable()
        {
            _webCamera.OnNewFrame -= OnNewFrame;
        }

        private void OnNewFrame(WebCamera camera, Texture2D motionTexture)
        {
            var targetZone = _zoneGetter.GetZone(camera);
            var pixels = GetRect(motionTexture, targetZone);
            var grayscaled = GrayScalePixels(pixels);

            _difference = CalcDifference(grayscaled, _background);

            if (_difference > _minDifference)
                OnMotionDetected?.Invoke();

            UpdateBackground(grayscaled);

            var zoneTexture = new Texture2D(targetZone.width, targetZone.height);
            zoneTexture.SetPixels(pixels);
            zoneTexture.Apply();

            OnFrameProcessed?.Invoke();
        }

        private float[] GrayScalePixels(Color[] pixels)
        {
            return pixels.Select(x => x.grayscale).ToArray();
        }

        private float CalcDifference(float[] pixels, float[] background)
        {
            if (background == null) return 0;
            if(pixels.Length != background.Length) return 0;

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
            if (_background.Length != pixels.Length) return;

            for (int i = 0; i < _background.Length; i++)
                _background[i] = (_background[i] + pixels[i]) / 2;
        }

        private Color[] GetRect(Texture2D motionTexture, RectInt rect)
        {
            return motionTexture.GetPixels(rect.x, rect.y, rect.width, rect.height);
        }
    }
}
