using UnityEngine;
using UnityEngine.UI;

namespace WebCamSystem
{
    [RequireComponent(typeof(RawImage))]
    [AddComponentMenu("WebCamSystem/Web Camera View")]
    public class WebCameraRawImage : MonoBehaviour
    {
        [SerializeField] private WebCamera _webCamera;
        [SerializeField] private Rect _uV = new Rect(0, 0, 1, 1);
        private RawImage _image;
        
        private void Awake()
        {
            _image = GetComponent<RawImage>();
            _image.uvRect = _uV;
            Apply(_webCamera.FullTexture, _webCamera.FlipY);
        }
                
        private void Apply(Texture texture, bool flipY)
        {
            _image.texture = texture;
            _image.material.mainTexture = texture;
            if (flipY)
            {
                _image.material.mainTextureOffset = new Vector2(1, 0);
                _image.material.mainTextureScale = new Vector2(-1, 1);
            }
            else
            {
                _image.material.mainTextureOffset = new Vector2(0, 0);
                _image.material.mainTextureScale = new Vector2(1, 1);
            }
        }
    }
}
