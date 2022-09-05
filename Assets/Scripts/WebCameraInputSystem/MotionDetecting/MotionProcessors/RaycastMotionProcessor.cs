using UnityEngine.EventSystems;
using UnityEngine;

namespace WebCameraInputSystem.MotionDetecting.MotionProcessors
{
    [AddComponentMenu("WebCameraInputSystem/Processors/Raycast Motion Processor")]
    public class RaycastMotionProcessor : MotionProcessor
    {
        protected override void OnDetect(MotionDetector detector, float difference)
        {
            var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            var ray = Camera.main.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out var hit))
            {
                var clickHandler = hit.collider.gameObject.GetComponent<IPointerClickHandler>();
                clickHandler.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }
    }
}
