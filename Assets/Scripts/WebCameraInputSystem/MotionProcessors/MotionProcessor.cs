using WebCameraInputSystem.MotionDetectors;
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
            _motionDetector.OnMotionDetected += OnMotionDetected;
        }

        private void OnDisable()
        {
            _motionDetector.OnMotionDetected -= OnMotionDetected;
        }

        private void OnMotionDetected()
        {
            OnDetect();
        }

        protected abstract void OnDetect();
    }
}
