using UnityEngine.Events;
using UnityEngine;

namespace WebCameraInputSystem.MotionDetection.MotionProcessors
{
    [AddComponentMenu("WebCamera InputSystem/Motions/Processors/Event Motion Processor")]
    public class EventMotionProcessor : MotionProcessor
    {
        [SerializeField] private UnityEvent<float> _onDetected;
        [SerializeField] private UnityEvent _onUnDetected;

        public event UnityAction<float> OnDetected
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
            _onDetected.Invoke(difference);
        }

        protected override void OnUnDetect(ZoneMotionDetector detector, float difference)
        {
            _onUnDetected.Invoke();
        }
    }
}
