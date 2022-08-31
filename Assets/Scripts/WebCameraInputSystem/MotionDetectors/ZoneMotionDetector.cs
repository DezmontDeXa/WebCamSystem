using UnityEngine;

namespace WebCameraInputSystem.MotionDetectors
{

    public class ZoneMotionDetector : MotionDetector
    {
        [SerializeField] private RectInt _zone;


        protected override RectInt GetZone() => _zone;
    }
}
