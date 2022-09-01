using UnityEngine;

namespace WebCameraInputSystem.ZoneGetters
{
    [AddComponentMenu("WebCameraInputSystem/Zone Getters/From RectTransform")]
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformZoneGetter : ZoneGetter
    {
        private RectTransform _rectTransform;
        private RectTransform _canvasRectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        }

        public override RectInt GetZone(WebCamera camera)
        {
            var bounds = GetRectTransformBounds(_rectTransform);
            var canvasBounds = GetRectTransformBounds(_canvasRectTransform);

            var frameSize = camera.MotionDetectFrameSize;

            var xMod = frameSize.x / canvasBounds.size.x;
            var yMod = frameSize.y / canvasBounds.size.y;

            var x = (bounds.center.x - bounds.extents.x) * xMod;
            var y = (bounds.center.y - bounds.extents.y) * yMod;
            var width = bounds.size.x * xMod;
            var height = bounds.size.y * yMod;

            return new RectInt((int)x, (int)y, (int)width, (int)height);
        }

        private static Bounds GetRectTransformBounds(RectTransform transform)
        {
            var WorldCorners = new Vector3[4];
            transform.GetWorldCorners(WorldCorners);
            var bounds = new Bounds(WorldCorners[0], Vector3.zero);
            for (int i = 1; i < 4; ++i)
                bounds.Encapsulate(WorldCorners[i]);
            return bounds;
        }
    }
}
