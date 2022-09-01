using UnityEngine;

namespace WebCameraInputSystem.ZoneGetters
{
    [AddComponentMenu("WebCameraInputSystem/Zone Getters/Manual")]
    public class ManualZoneGetter : ZoneGetter
    {
        [SerializeField] private RectInt _zone;

        public override RectInt GetZone(WebCamera camera) => _zone;
    }
}
