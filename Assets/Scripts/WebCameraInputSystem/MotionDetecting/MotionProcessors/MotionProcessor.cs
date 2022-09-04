using UnityEngine;

namespace WebCameraInputSystem.MotionDetecting.MotionProcessors
{
    public abstract class MotionProcessor : MonoBehaviour
    {
        [SerializeField] private MotionDetector _motionDetector;

        private void Awake()
        {
            _motionDetector ??= GetComponent<MotionDetector>();
        }

        private void OnEnable()
        {
            _motionDetector.OnFrameProcessed += OnFrameProcessed;
        }

        private void OnDisable()
        {
            _motionDetector.OnFrameProcessed -= OnFrameProcessed;
        }

        private void OnFrameProcessed(WebCamera camera, MotionDetector detector, float difference)
        {
            if (detector.HasMotion)
                OnDetect(camera, detector, difference);
            else
                OnUnDetect(camera, detector, difference);
        }

        protected virtual void OnDetect(WebCamera camera, MotionDetector detector, float difference) { }

        protected virtual void OnUnDetect(WebCamera camera, MotionDetector detector, float difference) { }
    }
}
