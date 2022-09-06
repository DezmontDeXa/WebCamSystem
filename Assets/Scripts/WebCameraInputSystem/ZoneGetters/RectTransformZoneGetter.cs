using UnityEngine;

namespace WebCameraInputSystem.ZoneGetters
{
    [AddComponentMenu("WebCamera InputSystem/Zones/RectTransform Zone Getter")]
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

        protected override RectInt GetZonePerform(Vector2Int originalFrameSize)
        {
            var bounds = GetRectTransformBounds(_rectTransform);
            var canvasBounds = GetRectTransformBounds(_canvasRectTransform);

            var xMod = originalFrameSize.x / canvasBounds.size.x;
            var yMod = originalFrameSize.y / canvasBounds.size.y;

            var x = (bounds.center.x - bounds.extents.x) * xMod;
            var y = (bounds.center.y - bounds.extents.y) * yMod;
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
