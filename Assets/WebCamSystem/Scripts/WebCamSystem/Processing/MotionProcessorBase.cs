using UnityEngine;

namespace WebCamSystem.Processing
{
    public abstract class MotionProcessorBase : MonoBehaviour
    {
        [SerializeField] private MotionDetector _motionDetector;

        private void OnEnable()
        {
            _motionDetector.OnDifferenceUpdated += OnDifferenceUpdated;
        }

        private void OnDisable()
        {
            _motionDetector.OnDifferenceUpdated -= OnDifferenceUpdated;
        }

        protected abstract void OnDifferenceUpdated(float difference);
    }
}