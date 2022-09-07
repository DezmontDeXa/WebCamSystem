﻿using System.Linq;
using UnityEngine;
using System;

namespace WebCameraInputSystem
{
    [Serializable]
    public class WebCameraName
    {
        public int SelectedIndex;
        public string[] Names => WebCamTexture.devices.Select(x=>x.name).ToArray();
    }
}
