using UnityEngine;

namespace WebCameraInputSystem.ZoneGetters
{
    [AddComponentMenu("WebCameraInputSystem/Zone Getters/Manual")]
    public class ManualZoneGetter : ZoneGetter
    {
        [SerializeField] private RectInt _zone;

        protected override RectInt GetZonePerform(WebCamera camera) => _zone;
    }
}
