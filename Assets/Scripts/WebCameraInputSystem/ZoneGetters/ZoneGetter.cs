using UnityEngine;

namespace WebCameraInputSystem.ZoneGetters
{
    public abstract class ZoneGetter : MonoBehaviour, IZoneGetter
    {
        // Create base realisation with bounding zone in camera bounds (Failed if rect out of screen)
        public RectInt GetZone(WebCamera camera)
        {
            var result = GetZonePerform(camera);
            result.ClampToBounds(new RectInt(0, 0, camera.MotionDetectFrameSize.x, camera.MotionDetectFrameSize.y));
            return result;
        }

        protected abstract RectInt GetZonePerform(WebCamera camera);
    }
}
