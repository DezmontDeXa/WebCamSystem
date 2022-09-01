using UnityEngine;
using UnityEngine.EventSystems;
using WebCameraInputSystem.MotionDetectors;

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

            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10);

            if (Physics.Raycast(ray, out var hit))
            {
                var clickHandler = hit.collider.gameObject.GetComponent<IPointerClickHandler>();
                clickHandler.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }    
    }
}
