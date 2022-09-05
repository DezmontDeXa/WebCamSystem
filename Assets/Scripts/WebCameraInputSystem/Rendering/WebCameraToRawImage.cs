using UnityEngine.UI;
using UnityEngine;
using System;

namespace WebCameraInputSystem.Drawing
{
    public class WebCameraToRawImage : WebCameraRenderer
    {
        [SerializeField] private RawImage _rawImage;

        protected override Material GetTargetMaterial()
        {
            return _rawImage.material;
        }
    }
}
