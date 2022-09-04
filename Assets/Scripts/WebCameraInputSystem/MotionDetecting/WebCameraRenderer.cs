using WebCameraInputSystem.ZoneGetters;
using UnityEngine;

namespace WebCameraInputSystem.MotionDetecting
{
    public class WebCameraRenderer : MonoBehaviour
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
            var targetZone = _zoneGetter.GetZone(camera);


        }
    }
}
