﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace WebCamSystem.Processing
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("WebCamSystem/Processors/Button Motion Processor")]
    public class ButtonMotionProcessor : MotionProcessorBase
    {
        [SerializeField] protected float _motionThreshold = 0.01f;
        [SerializeField] private Color _hasMotionColor;
        [SerializeField] private Color _noMotionColor;
        private Image _image;

        public event UnityAction OnCLick;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        protected override void OnDifferenceUpdated(float difference)
        {
            if (difference > _motionThreshold)
            {
                _image.color = _hasMotionColor;
                OnCLick?.Invoke();
            }
            else
                _image.color = _noMotionColor;
        }
    }
}
