using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

namespace WebCameraInputSystemOLD.MotionDetection.MotionProcessors
{
    [AddComponentMenu("WebCamera InputSystem/Motions/Processors/Debug Motion Processor")]
    public class DebugMotionProcessor : MotionProcessor
    {
        [SerializeField] private Image _colorIndicatorImage;
        [SerializeField] private Color _motionColor;
        [SerializeField] private Color _noMotionColor;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField, Range(0, 15)] private int decimals = 6;

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
                _scoreText.text = detector.Difference.ToString("0." + new string('0', decimals));
        }
    }
}
