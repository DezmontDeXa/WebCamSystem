using UnityEngine.UI;
using UnityEngine;

namespace WebCameraInputSystem.Rendering
{
    [AddComponentMenu("WebCamera InputSystem/Rendering/WebCam To RawImage")]
    public class WebCameraToRawImage : WebCameraRenderer
    {
        [SerializeField] private RawImage _rawImage;

        protected override void ApplyTexture(Texture2D croppedTexture)
        {
            _rawImage.texture = croppedTexture;
        }
        protected override Material GetTargetMaterial()
        {
            return _rawImage.material;
        }
    }
}
