using UnityEngine;

namespace WebCameraInputSystemOLD.ZoneGetters
{
    [AddComponentMenu("WebCamera InputSystem/Zones/Manual Zone Getter")]
    public class ManualZoneGetter : ZoneGetter
    {
        [SerializeField] private Rect _zone = new Rect(0,0,1,1);

        protected override RectInt GetZonePerform(Vector2Int originalFrameSize)
        {
            if(_zone.width==0 ||_zone.height==0 || _zone.x<0 || _zone.y<0)
                throw new System.Exception($"{gameObject.name}.ManualZoneGetter: Invalide zone parameters");

            return new RectInt((int)_zone.x, (int)_zone.y, (int)(originalFrameSize.x * _zone.width), (int)(originalFrameSize.y * _zone.height));
        }
    }
}
