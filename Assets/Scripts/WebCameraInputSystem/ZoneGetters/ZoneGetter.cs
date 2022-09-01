using UnityEngine;

namespace WebCameraInputSystem.ZoneGetters
{
    public abstract class ZoneGetter : MonoBehaviour, IZoneGetter
    {
        public abstract RectInt GetZone(WebCamera camera);
    }


}
