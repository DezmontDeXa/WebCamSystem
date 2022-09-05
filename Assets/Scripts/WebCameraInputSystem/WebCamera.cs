using WebCameraInputSystem.Utils;
using UnityEngine.Events;
using UnityEngine;

namespace WebCameraInputSystem
{
    [AddComponentMenu("WebCameraInputSystem/WebCamera")]
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private Vector2Int _requestedFrameSize = new Vector2Int(1920, 1080);
        [SerializeField] private int _requestedFps = 30;
        [SerializeField] private Vector2Int _detectFrameSize = new Vector2Int(192, 108);
        [SerializeField] private int _detectFps = 10;
        [SerializeField] private bool _flipY = true;
        private float _prevTime = 0;
        private WebCamTexture _webCamTexture;
        private Texture2D _motionTexture;

        public event UnityAction<WebCameraFrame> OnNewFrame;

        private void Awake()
        {
            var cams = FindObjectsOfType<WebCamera>();
            if (cams.Length > 1)
                Debug.LogWarning("Recommended to use only one Web Camera on scene");
            _webCamTexture = new WebCamTexture(_requestedFrameSize.x, _requestedFrameSize.y, _requestedFps);
            _motionTexture = new Texture2D(_detectFrameSize.x, _detectFrameSize.y);
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
            OnNewFrame?.Invoke(new WebCameraFrame(_webCamTexture, _motionTexture));
        }        
    }

    public class WebCameraFrame
    {
        public WebCamTexture FullTexture { get; }
        public Texture2D MotionTexture { get; }
        public Vector2Int FullTextureSize => new Vector2Int(FullTexture.width, FullTexture.height);
        public Vector2Int MotionTextureSize => new Vector2Int(MotionTexture.width, MotionTexture.height);

        public WebCameraFrame(WebCamTexture fullTexture, Texture2D motionTexture)
        {
            FullTexture = fullTexture;
            MotionTexture = motionTexture;
        }
    }
}
