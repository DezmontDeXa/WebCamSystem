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

        private void OnNewFrame(WebCameraFrame frame)
        {
            RectInt zone = new RectInt(0, 0, frame.FullTextureSize.x, frame.FullTextureSize.y);
            if (_zoneGetter != null)
                zone = _zoneGetter.GetZone(new Vector2Int(frame.FullTextureSize.x, frame.FullTextureSize.y));

            var pixels = frame.FullTexture.GetPixels(zone.x, zone.y, zone.width, zone.height);
            ApplyPixels(pixels, zone.width, zone.height);
        }

        protected abstract void Apply(WebCamTexture webCamTexture, RectInt zone);

        protected abstract void ApplyPixels(Color[] pixels, int width, int height);
    }
}
