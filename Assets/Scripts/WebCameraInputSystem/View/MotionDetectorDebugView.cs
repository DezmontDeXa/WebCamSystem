using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace WebCameraInputSystem.MotionDetectors
{
    [RequireComponent(typeof(MotionDetector))]
    public class MotionDetectorDebugView : MonoBehaviour
    {
        [SerializeField] private Image _colorIndicatorImage;
        [SerializeField] private Color _motionColor;
        [SerializeField] private Color _noMotionColor;
        [SerializeField] private TMP_Text _scoreText;
        private MotionDetector _motionDetector;

        private void Awake()
        {
            _motionDetector = GetComponent<MotionDetector>();
        }

        private void OnEnable()
        {
            _motionDetector.OnFrameProcessed += OnFrameProcessed;
        }

        private void OnDisable()
        {
            _motionDetector.OnFrameProcessed -= OnFrameProcessed;
        }

        private void OnFrameProcessed()
        {
            if (_colorIndicatorImage != null)
                _colorIndicatorImage.color = _motionDetector.HasMotion ? _motionColor : _noMotionColor;

            if(_scoreText!=null)
                _scoreText.text = _motionDetector.Difference.ToString();
        }
    }
}
