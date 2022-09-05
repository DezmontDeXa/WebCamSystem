using UnityEngine;

namespace WebCameraInputSystem.ZoneGetters
{
    public interface IZoneGetter
    {
        RectInt GetZone(Vector2Int frameSize);
    }
}