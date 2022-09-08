using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using WebCameraInputSystem.Core;

namespace WebCameraInputSystem
{
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private WebCameraName _webCameraName = new WebCameraName();
        [SerializeField] private Vector2Int _requestedResolution = new Vector2Int(1920, 1080);
        [SerializeField] private bool _flipY;
        [SerializeField] private int _requestedFps = 60;
        [SerializeField] private Vector2Int _motionDetectorResolution = new Vector2Int(192, 108);
        [SerializeField] private RawImage _debugImage;
        private WebCamTexture _webCam;
        private Texture2D _motionTexture;

        public event UnityAction<Texture, bool> OnInitialized;
        public event UnityAction<Texture2D, byte[]> OnMotionTextureUpdated;

        private void OnEnable()
        {
            _webCam = new WebCamTexture(_webCameraName.Names[_webCameraName.SelectedIndex], _requestedResolution.x, _requestedResolution.y, _requestedFps);
            _motionTexture = new Texture2D(_motionDetectorResolution.x, _motionDetectorResolution.y, TextureFormat.ARGB32, false);
            if (_debugImage != null)
            {
                _debugImage.texture = _motionTexture;
                _debugImage.material.mainTexture = _motionTexture;
                _debugImage.uvRect = new Rect(0, 0, _flipY ? -1 : 1, 1);
            }
            _webCam.Play();
            OnInitialized?.Invoke(_webCam, _flipY);
        }

        private void OnDisable()
        {
            _webCam.Stop();
            Destroy(_webCam);
        }

        private void Update()
        {
            if (_webCam.didUpdateThisFrame)
            {
                _webCam.ToTexture2D(ref _motionTexture, _flipY);
                var bytes = _motionTexture.GetRawTextureData();
                OnMotionTextureUpdated?.Invoke(_motionTexture, bytes);
            }
        }
    }
}
