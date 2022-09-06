using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

namespace WebCameraInputSystem
{
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private string _cameraName;
        [SerializeField] private Vector2Int _requestedResolution = new Vector2Int(1920, 1080);
        [SerializeField] private int _requestedFps = 60;
        [SerializeField] private bool _flipY = true;
        [SerializeField] private Vector2Int _motionDetectorResolution = new Vector2Int(192, 108);
        [SerializeField] private RawImage[] _targets;
        private WebCamTexture _webCam;
        private Texture2D _webCamTexture;

        public event UnityAction<Texture2D, byte[]> TextureUpdated;

        private void Awake()
        {
            Debug.Log($"Devices: \r\n{string.Join("\r\n", WebCamTexture.devices.Select(x => x.name))}");
        }

        private void OnEnable()
        {
            _webCam = new WebCamTexture(_cameraName, _requestedResolution.x, _requestedResolution.y, _requestedFps);
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
                    _webCamTexture = new Texture2D(_motionDetectorResolution.x, _motionDetectorResolution.y, TextureFormat.ARGB32, false);

                _webCam.ToTexture2D(ref _webCamTexture);
                var bytes = _webCamTexture.GetRawTextureData();
                TextureUpdated?.Invoke(_webCamTexture, bytes);
            }
        }
    }
}
