using UnityEngine;

namespace WebCameraInputSystem.MotionDetectors
{
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformMotionDetector : MotionDetector
    {
        private RectTransform _rectTransform;
        private RectTransform _canvasRectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        }

        protected override RectInt GetZone()
        {
            var bounds = GetRectTransformBounds(_rectTransform);
            var canvasBounds = GetRectTransformBounds(_canvasRectTransform);

            var frameSize = _webCamera.MotionDetectFrameSize;

            var xMod = frameSize.x / canvasBounds.size.x;
            var yMod = frameSize.y / canvasBounds.size.y;

            var x = (bounds.center.x - bounds.extents.x) * xMod;
            var y = (bounds.center.y - bounds.extents.y) * yMod;
            var width = bounds.size.x * xMod;
            var height = bounds.size.y * yMod;

            return new RectInt((int)x, (int)y, (int)width, (int)height);
        }

        public static Bounds GetRectTransformBounds(RectTransform transform)
        {
            Vector3[] WorldCorners = new Vector3[4];
            transform.GetWorldCorners(WorldCorners);
            Bounds bounds = new Bounds(WorldCorners[0], Vector3.zero);
            for (int i = 1; i < 4; ++i)
            {
                bounds.Encapsulate(WorldCorners[i]);
            }
            return bounds;
        }
    }
}
