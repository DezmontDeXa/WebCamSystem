using WebCameraInputSystem.Utils;
using UnityEngine.Events;
using UnityEngine;

namespace WebCameraInputSystem
{
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private Vector2Int _requestedFrameSize = new Vector2Int(1920, 1080);
        [SerializeField] private int _requestedFps = 30;
        [SerializeField] private Vector2Int _detectFrameSize = new Vector2Int(192, 108);
        [SerializeField] private int _detectFps = 10;
        [SerializeField] private bool _flipY;
        private float _prevTime = 0;
        private WebCamTexture _webCamTexture;
        private Texture2D _fullTexture;
        private Texture2D _motionTexture;

        public Texture FullTexture => _fullTexture;

        public event UnityAction<Texture, Texture2D> OnNewFrame;

        private void Awake()
        {
            var cams = FindObjectsOfType<WebCamera>();
            if (cams.Length > 1)
                Debug.LogWarning("Recommended to use only one Web Camera on scene");
            _webCamTexture = new WebCamTexture(_requestedFrameSize.x, _requestedFrameSize.y, _requestedFps);
            _motionTexture = new Texture2D(_detectFrameSize.x, _detectFrameSize.y);
            _fullTexture = new Texture2D(_requestedFrameSize.x, _requestedFrameSize.y);
        }

        private void OnEnable()
        {
            _webCamTexture.Play();
            _prevTime = Time.timeSinceLevelLoad;
        }

        private void OnDisable()
        {
            _webCamTexture.Stop();
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
            Alg.Resize(_webCamTexture, _motionTexture, _flipY);
            Alg.Resize(_webCamTexture, _fullTexture, _flipY);
            OnNewFrame?.Invoke(_fullTexture, _motionTexture);
        }
    }
}
