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

        public WebCamTexture WebCamTexture { get; private set; }
        public Texture2D MotionTexture { get; private set; }
        public Vector2Int MotionDetectFrameSize => _detectFrameSize;

        public event UnityAction<WebCamera> OnNewFrame;

        private void Awake()
        {
            var cams = FindObjectsOfType<WebCamera>();
            if (cams.Length > 1)
                Debug.LogWarning("It is recommended to use only one Web Camera Reader on scene");
            WebCamTexture = new WebCamTexture(_requestedFrameSize.x, _requestedFrameSize.y, _requestedFps);
            MotionTexture = new Texture2D(_detectFrameSize.x, _detectFrameSize.y);
        }

        private void OnEnable()
        {
            WebCamTexture.Play();
            _prevTime = Time.timeSinceLevelLoad;
        }

        private void OnDisable()
        {
            WebCamTexture.Stop();
        }

        private void Update()
        {
            if (!WebCamTexture.isPlaying) return;
            if (!WebCamTexture.didUpdateThisFrame) return;
            if (Time.timeSinceLevelLoad - _prevTime < 1f / _detectFps) return;
            _prevTime = Time.timeSinceLevelLoad;
            PerformFrame();
        }

        private void PerformFrame()
        {
            Alg.Resize(WebCamTexture, MotionTexture, _flipY);
            OnNewFrame?.Invoke(this);
        }        
    }
}
