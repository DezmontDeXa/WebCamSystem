using System.Collections;
using UnityEngine.Events;
using UnityEngine;

namespace WebCameraInputSystem
{
    public class WebCameraReader : MonoBehaviour
    {
        [SerializeField] private float _readingRate = 30;
        private WebCamTexture _webcamTexture;

        public Color[] LastFrame { get; private set; }
        public Vector2Int TextureSize { get; private set; }

        public event UnityAction<Color[]> OnNewFrameReaded;

        private void Awake()
        {
            var readers = FindObjectsOfType<WebCameraReader>();
            if (readers.Length > 1)
                Debug.LogWarning("It is recommended to use only one Web Camera Reader on scene");

            _webcamTexture = new WebCamTexture();
        }

        private void OnEnable()
        {
            _webcamTexture.Play();
            TextureSize = new Vector2Int(_webcamTexture.width, _webcamTexture.height);
            StartCoroutine(FrameReading());
        }

        private void OnDisable()
        {
            _webcamTexture.Stop();
        }

        private IEnumerator FrameReading()
        {
            while (enabled)
            {
                yield return new WaitForSecondsRealtime(1f / _readingRate);
                LastFrame = _webcamTexture.GetPixels();
                OnNewFrameReaded?.Invoke(LastFrame);
            }
        }
    }
}
