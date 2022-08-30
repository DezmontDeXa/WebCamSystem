using System.Collections.Generic;
using UnityEngine;

namespace WebCameraInputSystem
{
    public static class Extensions
    {
        public static Color[,] GetRect(this Color[] pixels, Vector2Int size, RectInt rect)
        {
            var frame = new Color[rect.width, rect.height];

            var width = size.x;
            var height = size.y;
            
            for (var y = rect.y; y < rect.y + rect.height; y++)
            {
                for (var x = rect.x; x < rect.x + rect.width; x++)
                {
                    frame[x - rect.x, y-rect.y] = pixels[x + (y * width)];                    
                }
            }

            return frame;
        }
        public static List<Color> GetRectPoints(this Color[] pixels, Vector2Int size, RectInt rect)
        {
            //var frame = new Color[rect.width, rect.height];
            List<Color> frame = new List<Color>(rect.width * rect.height);

            var width = size.x;
            var height = size.y;

            for (var y = rect.y; y < rect.y + rect.height; y++)
            {
                for (var x = rect.x; x < rect.x + rect.width; x++)
                {
                    frame.Add(pixels[x + (y * width)]);
                }
            }

            return frame;
        }
    }
}
