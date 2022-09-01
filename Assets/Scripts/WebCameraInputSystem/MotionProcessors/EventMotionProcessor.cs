using UnityEngine;
using UnityEngine.Events;

namespace WebCameraInputSystem.MotionProcessors
{
    [AddComponentMenu("WebCameraInputSystem/Processors/Event Motion Processor")]
    public class EventMotionProcessor : MotionProcessor
    {
        [SerializeField] private UnityEvent _onDetected;

        public event UnityAction OnDetected
        {
            add => _onDetected.AddListener(value);
            remove => _onDetected.RemoveListener(value);
        }

        protected override void OnDetect()
        {
            _onDetected.Invoke();
        }
    }
}
