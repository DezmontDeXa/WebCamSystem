using WebCameraInputSystem.Utils;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace WebCameraInputSystem
{
    [AddComponentMenu("WebCameraInputSystem/WebCamera")]
    public class WebCamera : MonoBehaviour
    {
        [SerializeField] private Vector2Int _requestedFrameSize = new Vector2Int(1920, 1080);
        [SerializeField] private int _requestedFps = 30;
        [SerializeField] private Vector2Int _motionDetectFrameSize = new Vector2Int(192, 108);
        [SerializeField] private int _detectionFps = 10;
        private WebCamTexture _webCamTexture;
        private float _prevTime = 0;
        private Texture2D _motionTexture;

        public WebCamTexture WebCamTexture => _webCamTexture;
        public Texture2D MotionTexture => _motionTexture;
        public Vector2Int MotionDetectFrameSize => _motionDetectFrameSize;

        public event UnityAction<WebCamera> OnNewFrame;

        private void Awake()
        {
            var cams = FindObjectsOfType<WebCamera>();
            if (cams.Length > 1)
                Debug.LogWarning("It is recommended to use only one Web Camera Reader on scene");
            _webCamTexture = new WebCamTexture(_requestedFrameSize.x, _requestedFrameSize.y, _requestedFps);
            _motionTexture = new Texture2D(_motionDetectFrameSize.x, _motionDetectFrameSize.y);
        }

        [Obsolete]
        private void OnEnable()
        {
            _webCamTexture.Play();
            _prevTime = Time.timeSinceLevelLoad;
        }

        private void OnDisable()
        {
            _webCamTexture.Stop();
        }

        [Obsolete]
        private void Update()
        {
            if (!_webCamTexture.isPlaying) return;
            if (!_webCamTexture.didUpdateThisFrame) return;
            if (Time.timeSinceLevelLoad - _prevTime < 1f / _detectionFps) return;
            _prevTime = Time.timeSinceLevelLoad;
            PerformFrame();
        }

        [Obsolete]
        private void PerformFrame()
        {
            Resize(_webCamTexture, _motionTexture, true);
            OnNewFrame?.Invoke(this);
        }

        public static void Resize(Texture original, Texture2D target, bool flipY = true, bool mipmap = true, FilterMode filter = FilterMode.Bilinear)
        {
            //create a temporary RenderTexture with the target size
            RenderTexture rt = RenderTexture.GetTemporary(target.width, target.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);

            //set the active RenderTexture to the temporary texture so we can read from it
            RenderTexture.active = rt;

            //Copy the texture data on the GPU - this is where the magic happens [(;]
            if (flipY)
                Graphics.Blit(original, rt, new Vector2(-1, 1), new Vector2(1, 0));
            else
                Graphics.Blit(original, rt);

            //resize the texture to the target values (this sets the pixel data as undefined)
            target.filterMode = filter;

            //reads the pixel values from the temporary RenderTexture onto the resized texture
            target.ReadPixels(new Rect(0.0f, 0.0f, target.width, target.height), 0, 0);

            //actually upload the changed pixels to the graphics card
            target.Apply();

            RenderTexture.ReleaseTemporary(rt);
        }

        private class TextureRectSubscriber
        {
            public UnityAction<Color[]> Action { get; }
            public RectInt TargetRect { get; }

            public TextureRectSubscriber(RectInt targetRect, UnityAction<Color[]> action)
            {
                TargetRect = targetRect;
                Action = action;
            }
        }
    }
}
