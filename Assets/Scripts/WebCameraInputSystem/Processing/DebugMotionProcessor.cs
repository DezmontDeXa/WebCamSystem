using UnityEngine;
using UnityEngine.UI;

namespace WebCameraInputSystem.Processing
{
    [RequireComponent(typeof(Image))]
    public class DebugMotionProcessor : MonoBehaviour
    {
        [SerializeField] private MotionDetector _motionDetector;
        [SerializeField] protected float _motionThreshold = 0.01f;
        [SerializeField] private Color _hasMotionColor;
        [SerializeField] private Color _noMotionColor;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            if (_motionDetector.Difference > _motionThreshold)
                _image.color = _hasMotionColor;
            else
                _image.color = _noMotionColor;
        }
    }
}
