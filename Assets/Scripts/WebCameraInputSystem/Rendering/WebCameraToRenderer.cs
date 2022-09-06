using UnityEngine;

namespace WebCameraInputSystem.Rendering
{
    [AddComponentMenu("WebCamera InputSystem/Rendering/WebCam To Renderer")]
    public class WebCameraToRenderer : WebCameraRenderer
    {
        [SerializeField] private Renderer _renderer;

        protected override void ApplyTexture(Texture2D croppedTexture)
        {
        }

        protected override Material GetTargetMaterial()
        {
            return _renderer.material;
        }
    }
}
