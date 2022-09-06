using WebCameraInputSystem.Utils;
using UnityEngine;

namespace WebCameraInputSystem.Drawing
{
    public abstract class WebCameraRenderer : MonoBehaviour
    {
        [SerializeField] private WebCamera _webCamera;
        [SerializeField] private Rect _webCameraRect = new Rect(0,0,1,1);
        private Material _material;
        private Texture2D _croppedTexture;

        private void OnEnable()
        {
            _webCamera.OnNewFrame += OnNewFrame;
            _material = GetTargetMaterial();
        }

        private void OnDisable()
        {
            _webCamera.OnNewFrame -= OnNewFrame;
            _material.mainTexture = null;
        }

        private void OnNewFrame(Texture cameraTexture, Texture2D motionTexture)
        {
            ApplyTextureWithCrop(cameraTexture);
        }

        private void ApplyTextureWithCrop(Texture texture)
        {
            if (_material == null) return;
            NormalizeWebCameraRect();
            Alg.Crop(texture, ref _croppedTexture, _webCameraRect);
            _material.mainTexture = _croppedTexture;
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

        protected abstract Material GetTargetMaterial();
    }
}
