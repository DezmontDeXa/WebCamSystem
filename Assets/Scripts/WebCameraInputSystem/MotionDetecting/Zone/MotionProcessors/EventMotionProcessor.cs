using UnityEngine.Events;
using UnityEngine;

namespace WebCameraInputSystem.MotionDetection.Zone.MotionProcessors
{
    public class EventMotionProcessor : MotionProcessor
    {
        [SerializeField] private UnityEvent _onDetected;
        [SerializeField] private UnityEvent _onUnDetected;

        public event UnityAction OnDetected
        {
            add => _onDetected.AddListener(value);
            remove => _onDetected.RemoveListener(value);
        }

        public event UnityAction OnUnDetected
        {
            add => _onUnDetected.AddListener(value);
            remove => _onUnDetected.RemoveListener(value);
        }

        protected override void OnDetect(ZoneMotionDetector detector, float difference)
        {
            _onDetected.Invoke();
        }

        protected override void OnUnDetect(ZoneMotionDetector detector, float difference)
        {
            _onUnDetected.Invoke();
        }
    }
}
