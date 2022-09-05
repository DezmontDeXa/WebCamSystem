using UnityEngine;

namespace WebCameraInputSystem.MotionDetection.Matrix
{
    public class MatrixMotion
    {
        public Vector2Int Cell { get; }
        public float Difference { get; }

        public MatrixMotion(Vector2Int cell, float difference)
        {
            Cell = cell;
            Difference = difference;
        }
    }
}
