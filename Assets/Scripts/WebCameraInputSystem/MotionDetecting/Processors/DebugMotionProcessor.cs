using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace WebCameraInputSystem.MotionDetection.MotionProcessors
{
    public class DebugMotionProcessor : MotionProcessor
    {
        [SerializeField] private Image _colorIndicatorImage;
        [SerializeField] private Color _motionColor;
        [SerializeField] private Color _noMotionColor;
        [SerializeField] private TMP_Text _scoreText;

        protected override void OnDetect(ZoneMotionDetector detector, float difference)
        {
            OnFrameProcessed(detector);
        }

        protected override void OnUnDetect(ZoneMotionDetector detector, float difference)
        {
            OnFrameProcessed(detector);
        }

        private void OnFrameProcessed(ZoneMotionDetector detector)
        {
            if (_colorIndicatorImage != null)
                _colorIndicatorImage.color = detector.HasMotion ? _motionColor : _noMotionColor;

            if (_scoreText != null)
                _scoreText.text = detector.Difference.ToString("0.000");
        }
    }
}
