using UnityEngine;

namespace WebCameraInputSystemOLD.ZoneGetters
{
    public abstract class ZoneGetter : MonoBehaviour, IZoneGetter
    {
        public RectInt GetZone(Vector2Int originalFrameSize)
        {
            var result = GetZonePerform(originalFrameSize);
            result.ClampToBounds(new RectInt(0, 0, originalFrameSize.x, originalFrameSize.y));
            return result;
        }

        protected abstract RectInt GetZonePerform(Vector2Int originalFrameSize);
    }
}
