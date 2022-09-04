using UnityEngine;

namespace WebCameraInputSystem
{
    [RequireComponent(typeof(WebCamera))]
    public class WebCameraRenderer : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        private WebCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<WebCamera>();
        }

        private void OnEnable()
        {
            if (_renderer == null) return;
            _renderer.material.mainTexture = _camera.WebCamTexture;
        }

        private void OnDisable()
        {
            if (_renderer == null) return;
            _renderer.material.mainTexture = null;
        }
    }
}
