﻿using UnityEngine;

namespace WebCameraInputSystemOLD.ZoneGetters
{
    public interface IZoneGetter
    {
        RectInt GetZone(Vector2Int frameSize);
    }
}