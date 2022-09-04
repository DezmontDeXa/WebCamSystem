using WebCameraInputSystem.ZoneGetters;
using UnityEngine;

namespace WebCameraInputSystem.Drawing
{
    public abstract class WebCameraDrawerBase : MonoBehaviour
    {
        [SerializeField] private WebCamera _webCamera;
        [SerializeField] private ZoneGetter _zoneGetter;

        private void OnEnable()
        {
            _webCamera.OnNewFrame += OnNewFrame;
        }

        private void OnDisable()
        {
            _webCamera.OnNewFrame -= OnNewFrame;
        }

        private void OnNewFrame(WebCamera camera)
        {
            RectInt zone = new RectInt(0, 0, camera.WebCamTexture.width, camera.WebCamTexture.height);
            if (_zoneGetter!=null)
                zone = _zoneGetter.GetZone(camera, new Vector2Int(camera.WebCamTexture.width, camera.WebCamTexture.height));

            var frame = _webCamera.WebCamTexture;
            var pixels = frame.GetPixels(zone.x, zone.y, zone.width, zone.height);
            ApplyPixels(pixels, zone.width, zone.height);
            
        }

        protected abstract void Apply(WebCamTexture webCamTexture, RectInt zone);
        protected abstract void ApplyPixels(Color[] pixels, int width, int height);
    }
}
