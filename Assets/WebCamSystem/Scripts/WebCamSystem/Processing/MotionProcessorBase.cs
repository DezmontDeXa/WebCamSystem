using UnityEngine;

namespace WebCamSystem.Processing
{
    public abstract class MotionProcessorBase : MonoBehaviour
    {
        [SerializeField] private MotionDetector _motionDetector;

        protected void OnEnable()
        {
            _motionDetector.OnDifferenceUpdated += OnDifferenceUpdated;
        }

        protected void OnDisable()
        {
            _motionDetector.OnDifferenceUpdated -= OnDifferenceUpdated;
        }

        protected abstract void OnDifferenceUpdated(float difference);
    }
}