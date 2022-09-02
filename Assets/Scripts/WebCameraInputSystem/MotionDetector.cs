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

        public event UnityAction<MotionDetector, float> OnFrameProcessed;

        public event UnityAction<MotionDetector, float> OnMotionDetected;

        private void OnEnable()
        {
            _webCamera.OnNewFrame += OnNewFrameOptimized;
        }

        private void OnDisable()
        {
            _webCamera.OnNewFrame -= OnNewFrameOptimized;
        }

        private void OnNewFrameOptimized(WebCamera camera)
        {
            var targetZone = _zoneGetter.GetZone(camera);
            var bytes = camera.MotionTexture.GetRawTextureData();
            var bytesOfZone = Crop(bytes, camera.MotionDetectFrameSize, targetZone);
            var grayscaled = GetGrayScale(bytesOfZone);

            _difference = CalcDifference(grayscaled, _background);

            if (_difference > _minDifference)
                OnMotionDetected?.Invoke(this, _difference);

            UpdateBackground(grayscaled);

            OnFrameProcessed?.Invoke(this, _difference);
        }

        private float[] GetGrayScale(byte[] bytes)
        {
            var result = new float[bytes.Length / 4];
            // .3f .59f .11f
            int byteIndex = 0;
            for (int pixel = 0; pixel < result.Length; pixel++)
            {
                float rawValue =
                    ((1f / 255f * bytes[byteIndex + 0]) * 0.3f) +
                    ((1f / 255f * bytes[byteIndex + 1]) * 0.59f) +
                    ((1f / 255f * bytes[byteIndex + 2]) * 0.11f);

                result[pixel] = MathF.Round( rawValue, 2);
                byteIndex += 4;
            }
            return result;
        }

        private byte[] Crop(byte[] bytes, Vector2Int textureSize, RectInt targetZone)
        {
            byte[] result = new byte[targetZone.width * targetZone.height * 4];

            for (int lineIndex = 0; lineIndex < targetZone.y; lineIndex++)
            {
                int firstIndex = (textureSize.x * (targetZone.y + lineIndex) + targetZone.x) * 4;
                int lineLenght = targetZone.width * 4;
                for (int i = 0; i < lineLenght; i++)
                    result[i] = bytes[firstIndex + i];
            }
            return result;
        }

        private void OnNewFrame(WebCamera camera)
        {
            try
            {
                var targetZone = _zoneGetter.GetZone(camera);
                var pixels = GetRect(camera.MotionTexture, targetZone);
                var grayscaled = GrayScalePixels(pixels);

                _difference = CalcDifference(grayscaled, _background);

                if (_difference > _minDifference)
                    OnMotionDetected?.Invoke(this, _difference);

                UpdateBackground(grayscaled);

                var zoneTexture = new Texture2D(targetZone.width, targetZone.height);
                zoneTexture.SetPixels(pixels);
                zoneTexture.Apply();

                OnFrameProcessed?.Invoke(this, _difference);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private float[] GrayScalePixels(Color[] pixels)
        {
            return pixels.Select(x => x.grayscale).ToArray();
        }

        private float CalcDifference(float[] pixels, float[] background)
        {
            if (background == null) return 0;
            if (pixels.Length != background.Length) return 0;

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
                _background[i] = MathF.Round( (_background[i] + pixels[i]) / 2, 2);
        }

        private Color[] GetRect(Texture2D motionTexture, RectInt rect)
        {
            return motionTexture.GetPixels(rect.x, rect.y, rect.width, rect.height);
        }
    }
}
