using WebCameraInputSystem.Utils;
using UnityEngine;

namespace WebCameraInputSystem
{
    [RequireComponent(typeof(WebCamera))]
    public class WebCameraView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TargetTexture _textureType;
        private WebCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<WebCamera>();
        }

        private void Start()
        {
            if (_renderer == null) return;
            if (_textureType == TargetTexture.OriginalTexture)
                _renderer.material.mainTexture = _camera.WebCamTexture;
        }

        private void OnEnable()
        {
            if (_renderer == null) return;
            _camera.OnNewFrame += OnNewFrame;
        }

        private void OnDisable()
        {
            if (_renderer == null) return;
            _camera.OnNewFrame -= OnNewFrame;
        }

        private void OnNewFrame(WebCamera camera, Texture2D texture)
        {
            if (_renderer == null) return;
            if (_textureType == TargetTexture.MotionTexture)
                _renderer.material.mainTexture = texture;
        }
    }
}
