using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace WebCameraInputSystem
{
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private WebCameraName _webCameraName = new WebCameraName();
        [SerializeField] private Vector2Int _requestedResolution = new Vector2Int(1920, 1080);
        [SerializeField] private bool _flipY;
        [SerializeField] private int _requestedFps = 60;
        [SerializeField] private Vector2Int _motionDetectorResolution = new Vector2Int(192, 108);
        [SerializeField] private RawImage[] _targets;
        [SerializeField] private RawImage _debugImage;
        private WebCamTexture _webCam;
        private Texture2D _webCamTexture;


        public event UnityAction<Texture2D, byte[]> TextureUpdated;

        private void Awake()
        {
            Debug.Log($"Devices: \r\n{string.Join("\r\n", WebCamTexture.devices.Select(x => x.name))}");
        }

        private void OnEnable()
        {
            _webCam = new WebCamTexture(_webCameraName.Names[_webCameraName.SelectedIndex], _requestedResolution.x, _requestedResolution.y, _requestedFps);
            foreach (var img in _targets)
            {
                img.texture = _webCam;
                img.material.mainTexture = _webCam;
                if (_flipY)
                {
                    img.material.mainTextureOffset = new Vector2(1, 0);
                    img.material.mainTextureScale = new Vector2(-1, 1);
                }
            }
            _webCam.Play();
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
                if (_webCamTexture == null)
                {
                    _webCamTexture = new Texture2D(_motionDetectorResolution.x, _motionDetectorResolution.y, TextureFormat.ARGB32, false);
                    if (_debugImage != null)
                    {
                        _debugImage.texture = _webCamTexture;
                        _debugImage.material.mainTexture = _webCamTexture;
                        _debugImage.uvRect = new Rect(0, 0, _flipY ? -1 : 1, 1);
                    }
                }
                _webCam.ToTexture2D(ref _webCamTexture, _flipY);
                var bytes = _webCamTexture.GetRawTextureData();
                TextureUpdated?.Invoke(_webCamTexture, bytes);
            }
        }
    }
}
