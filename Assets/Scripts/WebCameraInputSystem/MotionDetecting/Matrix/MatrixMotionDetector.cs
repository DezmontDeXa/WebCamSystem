using System.Collections.Generic;
using WebCameraInputSystem.Utils;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace WebCameraInputSystem.MotionDetection.Matrix
{
    public class MatrixMotionDetector : MonoBehaviour
    {
        [SerializeField] private WebCamera _webCamera;
        [SerializeField] private float _minDifference = 0.01f;
        private float[,] _background;

        public event UnityAction<float[,], MatrixMotion[]> OnFrameProcessed;

        private void Awake()
        {
            var cams = FindObjectsOfType<MatrixMotionDetector>();
            if (cams.Length > 1)
                Debug.LogWarning("Recommended to use only one Matrix Motion Detector on scene");
        }

        private void OnEnable()
        {
            _webCamera.OnNewFrame += OnNewFrame;
        }

        private void OnDisable()
        {
            _webCamera.OnNewFrame -= OnNewFrame;
        }

        private void OnNewFrame(WebCamTexture frame, Texture2D motionFrame)
        {
            var bytes = motionFrame.GetRawTextureData();
            var grayscaled = Alg.GetGrayScaleMatrix(bytes, new Vector2Int(motionFrame.width, motionFrame.height));
            var difference = Alg.CalcDifferenceMatrix(grayscaled, _background);
            OnFrameProcessed?.Invoke(difference, GetMotions(difference, _minDifference));
            UpdateBackground(grayscaled);
        }

        private MatrixMotion[] GetMotions(float[,] difference, float minDifference)
        {
            var motions = new List<MatrixMotion>();
            for (int x = 0; x < difference.GetLength(0); x++)
                for (int y = 0; y < difference.GetLength(1); y++)
                    if (difference[x, y] > minDifference)
                        motions.Add(new MatrixMotion(new Vector2Int(x, y), difference[x, y]));
            return motions.ToArray();
        }

        private void UpdateBackground(float[,] grayscaled)
        {
            if (_background == null)
            {
                _background = grayscaled;
                return;
            }
            if (_background.Length != grayscaled.Length) return;

            for (var i = 0; i < _background.GetLength(0); i++)
                for (var ii = 0; ii < _background.GetLength(1); ii++)
                    _background[i, ii] = MathF.Round(Alg.SquareMediateValue(_background[i, ii], grayscaled[i, ii]), 3);
            //_background[i] = MathF.Round(Alg.MediateValue(_background[i], pixels[i]), 3);
        }
    }
}
