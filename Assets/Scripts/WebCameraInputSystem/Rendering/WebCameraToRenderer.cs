using UnityEngine;

namespace WebCameraInputSystem.Drawing
{
    public class WebCameraToRenderer : WebCameraRenderer
    {
        [SerializeField] private Renderer _renderer;

        protected override Material GetTargetMaterial()
        {
            return _renderer.material;
        }
    }
}
