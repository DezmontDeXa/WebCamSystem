using WebCameraInputSystem.Utils;
using UnityEngine;

namespace WebCameraInputSystem.Rendering
{
    public abstract class WebCameraRenderer : MonoBehaviour
    {
        [SerializeField] private WebCamera _webCamera;
        [SerializeField] private Rect _webCameraRect = new Rect(0,0,1,1);
        private Material _material;
        private RectInt? _rect;
        private Texture2D _croppedTexture;

        private void Awake()
        {
            _material = GetTargetMaterial();
        }

        private void OnEnable()
        {
            _webCamera.OnNewFrame += OnNewFrame;
        }

        private void OnDisable()
        {
            _webCamera.OnNewFrame -= OnNewFrame;
            _material.mainTexture = null;
        }

        private void OnNewFrame(Texture cameraTexture, Texture2D motionTexture)
        {
            PrepareTexture(cameraTexture);

            ApplyTexture(_croppedTexture);
        }

        private void PrepareTexture(Texture texture)
        {
            if (_material == null) return;
            NormalizeWebCameraRect();

            if (_rect == null)
            {
                _rect = new RectInt(
                    (int)(texture.width * _webCameraRect.x),
                    (int)(texture.height * _webCameraRect.y),
                    (int)(texture.width * _webCameraRect.width),
                    (int)(texture.height * _webCameraRect.height));
                _rect.Value.ClampToBounds(new RectInt(0, 0, texture.width, texture.height));
                _croppedTexture = new Texture2D(_rect.Value.width, _rect.Value.height);
                _material.mainTexture = _croppedTexture;
            }

            Alg.Crop(texture, ref _croppedTexture, _rect.Value);
        }

        private void NormalizeWebCameraRect()
        {
            if (_webCameraRect.width > 1f) _webCameraRect.width = 1f;
            if (_webCameraRect.height > 1f) _webCameraRect.height = 1f;
            if (_webCameraRect.width < 0) _webCameraRect.width = 0f;
            if (_webCameraRect.height < 0f) _webCameraRect.height = 0f;

            if (_webCameraRect.x < 0)
                _webCameraRect.x = 0;
            if (_webCameraRect.y < 0)
                _webCameraRect.y = 0;

        }

        protected abstract void ApplyTexture(Texture2D croppedTexture);

        protected abstract Material GetTargetMaterial();
    }
}
