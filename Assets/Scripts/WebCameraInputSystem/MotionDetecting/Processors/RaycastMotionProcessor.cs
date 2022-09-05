using UnityEngine;
using UnityEngine.Events;

namespace WebCameraInputSystem.MotionDetection.MotionProcessors
{
    public class RaycastMotionProcessor : MotionProcessor
    {
        public event UnityAction<RaycastHit> OnHit;

        protected override void OnDetect(ZoneMotionDetector detector, float difference)
        {
            var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            var ray = Camera.main.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out var hit))
                OnHit?.Invoke(hit);
        }
    }
}
