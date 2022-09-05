using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

namespace WebCameraInputSystem.MotionDetection.MotionProcessors
{
    public class GraphicRaycastMotionProcessor : MotionProcessor
    {
        [SerializeField] private GraphicRaycaster _graphicRaycaster;

        public event UnityAction<List<RaycastResult>> OnHit;

        private void Awake()
        {
            _graphicRaycaster ??= GetComponentInParent<GraphicRaycaster>();
        }

        protected override void OnDetect(ZoneMotionDetector detector, float difference)
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = transform.position;// Camera.main.WorldToScreenPoint(transform.position);
            var results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(eventData, results);
            if (results.Count > 0)
                OnHit?.Invoke(results);
        }
    }
}
