using System.Linq;
using UnityEngine;
using System;

namespace WebCamSystem.Core
{
    [Serializable]
    public class WebCameraName
    {
        public int SelectedIndex;

        public string[] Names => WebCamTexture.devices.Select(x=>x.name).ToArray();

        public WebCameraName(string cameraName)
        {
            SelectedIndex = IndexOf(cameraName);
        }

        public WebCameraName()
        {
        }

        private int IndexOf(string cameraName)
        {
            for (var i = 0; i < Names.Length; i++)
                if (Names[i] == cameraName)
                    return i;
            return -1;
        }
    }
}
