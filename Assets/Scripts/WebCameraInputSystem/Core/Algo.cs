using System;
using UnityEngine;
using System.Linq;

namespace WebCameraInputSystem.Core
{
    public static class Algo
    {
        public static void ToTexture2D(this Texture original, ref Texture2D output, bool flipY)
        {
            var rt = RenderTexture.GetTemporary(output.width, output.height, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            RenderTexture.active = rt;

            Graphics.Blit(
                original,
                rt,
                new Vector2((flipY ? -1 : 1), 1), //(float)original.width/output.width    (float)original.height / output.height
                new Vector2((flipY ? 1 : 0), 0));

            output.filterMode = FilterMode.Point;
            output.ReadPixels(new Rect(0.0f, 0.0f, output.width, output.height), 0, 0);
            output.Apply();
            RenderTexture.ReleaseTemporary(rt);
        }

        public static float[] GetGrayScaleFromBytes(byte[] bytes)
        {
            var result = new float[bytes.Length / 4];
            var byteIndex = 0;
            for (var pixel = 0; pixel < result.Length; pixel++)
            {
                var rawValue =
                    (bytes[byteIndex + 1] * 0.21f) +
                    (bytes[byteIndex + 2] * 0.72f) +
                    (bytes[byteIndex + 3] * 0.07f);
                rawValue /= 255f;

                result[pixel] = MathF.Round(rawValue, 2);
                byteIndex += 4;
            }
            return result;
        }

        public static float CalcDifference(float[] pixels, float[] background, float _minPixelDifference)
        {
            if (background == null) return 0;
            if (pixels.Length != background.Length) return 0;

            var differentCount = 0;
            for (var i = 0; i < background.Length; i++)
                if (MathF.Abs(MathF.Abs(background[i]) - MathF.Abs(pixels[i])) > _minPixelDifference)
                    differentCount++;

            return (float)differentCount / background.Length;
        }

        public static byte[] CropByBytes(byte[] bytes, Vector2Int textureSize, RectInt targetZone)
        {
            targetZone.ClampToBounds(new RectInt(0, 0, textureSize.x, textureSize.y));
            var result = new byte[targetZone.width * targetZone.height * 4];

            var lastIndex = 0;
            for (var lineIndex = 0; lineIndex < targetZone.height; lineIndex++)
            {
                var firstIndex = (textureSize.x * (targetZone.y + lineIndex) + targetZone.x) * 4;
                var lineLenght = targetZone.width * 4;
                for (int i = 0; i < lineLenght; i++)
                    result[i + lastIndex] = bytes[firstIndex + i];

                lastIndex += lineLenght;
            }
            return result;
        }

        public static float SquareMediateValue(params float[] values)
        {
            return MathF.Sqrt(values.Sum(x => MathF.Pow(x, 2)) / values.Length);
        }
    }
}
