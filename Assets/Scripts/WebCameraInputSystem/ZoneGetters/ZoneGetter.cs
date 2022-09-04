using UnityEngine;

namespace WebCameraInputSystem.ZoneGetters
{
    public abstract class ZoneGetter : MonoBehaviour, IZoneGetter
    {
        public RectInt GetZone(WebCamera camera, Vector2Int originalFrameSize)
        {
            var result = GetZonePerform(camera, originalFrameSize);
            result.ClampToBounds(new RectInt(0, 0, originalFrameSize.x, originalFrameSize.y));
            return result;
        }

        protected abstract RectInt GetZonePerform(WebCamera camera, Vector2Int originalFrameSize);
    }
}
