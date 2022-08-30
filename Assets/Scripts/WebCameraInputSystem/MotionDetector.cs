using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace WebCameraInputSystem
{
    public class MotionDetector : MonoBehaviour
    {
        [SerializeField] private WebCameraReader _reader;
        [SerializeField] private RectInt _zoneRect;
        [SerializeField] private float _threshold = 0.2f;
        [SerializeField] private float _difference = 0f;
        private List<float> _background;

        private void OnEnable()
        {
            _reader.OnNewFrameReaded += OnNewFrameReaded;
        }

        private void OnDisable()
        {
            _reader.OnNewFrameReaded -= OnNewFrameReaded;
        }

        private void OnNewFrameReaded(Color[] colorArray)
        {
            if (_reader.TextureSize.x * _reader.TextureSize.y != colorArray.Length)
                throw new IndexOutOfRangeException("The frame size does not match the size of the web camera texture.");

            var framePoints = colorArray.GetRectPoints(_reader.TextureSize, _zoneRect).Select(x=>x.grayscale).ToList();

            if (_background != null)
            {
                _difference = 0;
                for (int i = 0; i < framePoints.Count; i++)
                {
                    if (framePoints[i] > _background[i])
                        if (framePoints[i] - _background[i] > _threshold)
                            _difference++;

                    if (framePoints[i] < _background[i])
                        if ( _background[i] - framePoints[i] > _threshold)
                            _difference++;
                }
                _difference /= framePoints.Count;
            }

            UpdateBackground(framePoints);
        }

        private void UpdateBackground(List<float> points)
        {
            if (_background == null)
                _background = new List<float>(points);
            else
                for (var i = 0; i < points.Count; i++)
                    _background[i] = (_background[i] + points[i]) / 2;
        }
    }
}
