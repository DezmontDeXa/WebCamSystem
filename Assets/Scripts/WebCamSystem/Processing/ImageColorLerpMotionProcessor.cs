using UnityEngine;
using UnityEngine.UI;

namespace WebCamSystem.Processing
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("WebCamSystem/Processors/Image Color Lerp Processor")]
    public class ImageColorLerpMotionProcessor : MonoBehaviour
    {
        [SerializeField] private MotionDetector _motionDetector;
        [SerializeField] private Color _maxMotionColor;
        [SerializeField] private Color _minMotionColor;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            _image.color = Color.Lerp(_minMotionColor, _maxMotionColor, _motionDetector.Difference);
        }
    }
}
