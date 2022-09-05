using System;
using UnityEngine;
using WebCameraInputSystem.Utils;

namespace WebCameraInputSystem.Drawing
{
    public abstract class WebCameraRenderer : MonoBehaviour
    {
        [SerializeField] private WebCamera _webCamera;
        [SerializeField] private Rect _webCameraRect = new Rect(0,0,1,1);
        private Material _material;

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

        private void OnNewFrame(WebCamTexture cameraTexture, Texture2D motionTexture)
        {
            ApplyTextureWithCrop(cameraTexture);
        }

        private void ApplyTexture(WebCamTexture texture)
        {
            var mat = GetTargetMaterial();
            mat.mainTexture = texture;
            mat.mainTextureOffset = new Vector2(_webCameraRect.x, _webCameraRect.y);
            mat.mainTextureScale = new Vector2(_webCameraRect.width, _webCameraRect.height);
        }

        private void ApplyTextureWithCrop(WebCamTexture texture)
        {
            if (_material == null) return;
           Alg.Crop(texture, _material.mainTexture, _webCameraRect, true);
        }

        protected abstract Material GetTargetMaterial();
    }
}
