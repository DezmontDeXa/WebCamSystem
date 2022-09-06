using WebCameraInputSystemOLD.Utils;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace WebCameraInputSystemOLD
{
    [AddComponentMenu("WebCamera InputSystem/WebCamera")]
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private string _cameraName;
        [SerializeField] private Vector2Int _requestedFrameSize = new Vector2Int(1920, 1080);
        [SerializeField] private int _requestedFps = 30;
        [SerializeField] private Vector2Int _detectFrameSize = new Vector2Int(192, 108);
        [SerializeField] private int _detectFps = 10;
        [SerializeField] private bool _flipY;
        private float _prevTime = 0;
        private WebCamTexture _webCamTexture;
        private Texture2D _fullTexture;
        private Texture2D _motionTexture;

        [SerializeField] private RawImage[] _rawImages;
        public Texture FullTexture => _fullTexture;

        public event UnityAction<Texture, Texture2D> OnNewFrame;

        private void Awake()
        {
            Debug.Log("Cameras: " + string.Join("\r\n", WebCamTexture.devices.Select(x=>x.name)));

            var cams = FindObjectsOfType<WebCamera>();
            if (cams.Length > 1)
                Debug.LogWarning("Recommended to use only one Web Camera on scene");
            _motionTexture = new Texture2D(_detectFrameSize.x, _detectFrameSize.y);
            _fullTexture = new Texture2D(_requestedFrameSize.x, _requestedFrameSize.y);
        }

        private void OnEnable()
        {
            _webCamTexture = new WebCamTexture(_cameraName, _requestedFrameSize.x, _requestedFrameSize.y, _requestedFps);
            foreach (var raw in _rawImages)
            {
                raw.texture = _webCamTexture;
                raw.material.mainTexture = _webCamTexture;
            }

            _webCamTexture.Play();
            _prevTime = Time.timeSinceLevelLoad;
        }

        private void OnDisable()
        {
            _webCamTexture.Stop();
            Destroy(_webCamTexture);
        }

        private void Update()
        {
            if (!_webCamTexture.isPlaying) return;
            if (!_webCamTexture.didUpdateThisFrame) return;
            if (Time.timeSinceLevelLoad - _prevTime < 1f / _detectFps) return;
            _prevTime = Time.timeSinceLevelLoad;
            PerformFrame();
        }

        private void PerformFrame()
        {
            AlgOld.Resize(_webCamTexture, _motionTexture, _flipY);
            AlgOld.Resize(_webCamTexture, _fullTexture, _flipY);
            OnNewFrame?.Invoke(_fullTexture, _motionTexture);
        }
    }
}
