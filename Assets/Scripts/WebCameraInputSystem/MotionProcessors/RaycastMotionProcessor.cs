using WebCameraInputSystem.MotionDetectors;
using UnityEngine.EventSystems;
using UnityEngine;

namespace WebCameraInputSystem.MotionProcessors
{
    [RequireComponent(typeof(MotionDetector))]
    public class RaycastMotionProcessor : MonoBehaviour
    {
        private MotionDetector _motionDetector;

        private void Awake()
        {
            _motionDetector = GetComponent<MotionDetector>();
        }

        private void OnEnable()
        {
            _motionDetector.OnMotionDetected += OnMotionDetected;
        }

        private void OnDisable()
        {
            _motionDetector.OnMotionDetected -= OnMotionDetected;
        }

        private void OnMotionDetected()
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
