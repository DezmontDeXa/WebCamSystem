using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace WebCameraInputSystem.MotionDetecting.MotionProcessors
{
    [AddComponentMenu("WebCameraInputSystem/Processors/Graphic Raycast Motion Processor")]
    public class GraphicRaycastMotionProcessor : MotionProcessor
    {
        [SerializeField] private GraphicRaycaster _graphicRaycaster;

        protected override void OnDetect(MotionDetector detector, float difference)
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = transform.position;// Camera.main.WorldToScreenPoint(transform.position);
            var results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(eventData, results);
            foreach (RaycastResult result in results)
            {
                var clickHandler = result.gameObject.GetComponent<IPointerClickHandler>();
                clickHandler?.OnPointerClick(new PointerEventData(EventSystem.current));

                Debug.Log(result.gameObject);
            }
        }
    }
}
