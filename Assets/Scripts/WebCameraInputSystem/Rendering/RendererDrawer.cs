using UnityEngine;

namespace WebCameraInputSystem.Drawing
{
    public class RendererDrawer : WebCameraDrawerBase
    {
        [SerializeField] private Renderer _renderer;
        private Texture2D _texture;


        protected override void Apply(WebCamTexture webCamTexture, RectInt zone)
        {
            _renderer.material.mainTexture = webCamTexture;
        }

        protected override void ApplyPixels(Color[] pixels, int width, int height)
        {
            if(_texture==null)
                _texture = new Texture2D(width, height);
            if(_texture.width !=width || _texture.height!=height)
            {
                Destroy(_texture);
                _texture = new Texture2D(width, height);
            }
            _texture.SetPixels(pixels);
            _texture.Apply();
            _renderer.material.mainTexture = _texture;
        }
    }
}
