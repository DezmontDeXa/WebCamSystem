using UnityEngine;

namespace WebCameraInputSystem.MotionProcessors
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

        private void OnFrameProcessed(MotionDetector detector, float difference)
        {
            if (detector.HasMotion) 
                OnDetect(detector, difference);
            else
                OnUnDetect(detector, difference);
        }

        private void OnDetect(MotionDetector detector, float difference)
        {
            AfterOnDetect(detector, difference);
        }

        private void OnUnDetect(MotionDetector detector, float difference)
        {
            AfterOnUnDetect(detector, difference);
        }

        protected virtual void AfterOnDetect(MotionDetector detector, float difference)
        {

        }

        protected virtual void AfterOnUnDetect(MotionDetector detector, float difference)
        {

        }
    }
}
