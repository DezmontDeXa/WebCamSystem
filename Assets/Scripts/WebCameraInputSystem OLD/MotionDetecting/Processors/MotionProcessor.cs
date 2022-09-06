using UnityEngine;

namespace WebCameraInputSystemOLD.MotionDetection.MotionProcessors
{
    public abstract class MotionProcessor : MonoBehaviour
    {
        [SerializeField] private ZoneMotionDetector _motionDetector;

        private void Awake()
        {
            _motionDetector ??= GetComponent<ZoneMotionDetector>();
        }

        private void OnEnable()
        {
            _motionDetector.OnFrameProcessed += OnFrameProcessed;
        }

        private void OnDisable()
        {
            _motionDetector.OnFrameProcessed -= OnFrameProcessed;
        }

        private void OnFrameProcessed(ZoneMotionDetector detector, float difference)
        {
            if (detector.HasMotion)
                OnDetect(detector, difference);
            else
                OnUnDetect(detector, difference);
        }

        protected virtual void OnDetect(ZoneMotionDetector detector, float difference) { }

        protected virtual void OnUnDetect(ZoneMotionDetector detector, float difference) { }
    }
}
