using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace WebCameraInputSystem.MotionProcessors
{
    [AddComponentMenu("WebCameraInputSystem/Processors/Graphic Raycast Motion Processor")]
    public class GraphicRaycastMotionProcessor : MotionProcessor
    {
        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        protected override void OnDetect()
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
