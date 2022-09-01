using UnityEngine;

namespace WebCameraInputSystem.ZoneGetters
{
    public interface IZoneGetter
    {
        RectInt GetZone(WebCamera camera);
    }
}