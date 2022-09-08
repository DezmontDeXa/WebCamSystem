using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WebCamSystem.Processing
{
    [RequireComponent(typeof(Image))]
    public class FillingMotionProcessor : MotionProcessorBase
    {
        [SerializeField] private float _motionThreshold = 0.05f;
        [SerializeField] private float _speed = 0.01f;
        private Image _image;

        public event UnityAction OnFilled;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _image.fillAmount = 0;
        }

        protected override void OnDifferenceUpdated(float difference)
        {
            if (difference > _motionThreshold)
                _image.fillAmount += difference * _speed * Time.deltaTime;

            if (_image.fillAmount >= 1f)
                OnFilled?.Invoke();

        }
    }
}
