using System;
using UnityEngine;
using UnityEngine.UI;

namespace WebCameraInputSystem
{
    [RequireComponent(typeof(RectTransform))]
    public class MotionDetector : MonoBehaviour
    {
        [SerializeField] private WebCamera _webCam;
        [SerializeField, Range(0.01f, 1f)] private float _minPixelDiff = 0.05f;
        [SerializeField][ReadOnly] private float _difference;
        private RectTransform _canvasRectTransform;
        private float[] _background;
        private RectTransform _zone;

        public float Difference => _difference;

        private void Awake()
        {
            _zone = GetComponent<RectTransform>();
            _canvasRectTransform = _zone.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _webCam.TextureUpdated += TextureUpdated;
        }

        private void OnDisable()
        {
            _webCam.TextureUpdated -= TextureUpdated;
        }

        private void TextureUpdated(Texture2D texture, byte[] allBytes)
        {
            var zoneBytes = GetZoneBytes(allBytes, new Vector2Int(texture.width, texture.height));
            var grayscale = Algo.GetGrayScaleFromBytes(zoneBytes);
            _difference = Algo.CalcDifference(grayscale, _background, _minPixelDiff);
            UpdateBackground(grayscale);
        }

        private byte[] GetZoneBytes(byte[] allBytes, Vector2Int size)
        {
            var zone = GetZone(size);

            return Algo.CropByBytes(allBytes, size, zone);
        }

        private void UpdateBackground(float[] grayscale)
        {
            if (_background == null)
            {
                _background = grayscale;
                return;
            }
            if (_background.Length != grayscale.Length)
            {
                _background = grayscale;
                return;
            }
            for (int i = 0; i < _background.Length; i++)
                _background[i] = Algo.SquareMediateValue(_background[i], grayscale[i]);
        }

        private RectInt GetZone(Vector2Int originalFrameSize)
        {
            var bounds = GetRectTransformBounds(_zone);
            var canvasBounds = GetRectTransformBounds(_canvasRectTransform);
            var xMod = originalFrameSize.x / canvasBounds.size.x;
            var yMod = originalFrameSize.y / canvasBounds.size.y;
            var x = (bounds.min.x) * xMod;
            var y = (bounds.min.y) * yMod;
            var width = bounds.size.x * xMod;
            var height = bounds.size.y * yMod;

            return new RectInt((int)x, (int)y, (int)width, (int)height);
        }

        private static Bounds GetRectTransformBounds(RectTransform transform)
        {
            var worldCorners = new Vector3[4];
            transform.GetWorldCorners(worldCorners);
            var bounds = new Bounds(worldCorners[0], Vector3.zero);
            for (int i = 1; i < 4; ++i)
                bounds.Encapsulate(worldCorners[i]);
            return bounds;
        }
    }
}
