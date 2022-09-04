using System;
using UnityEngine;

namespace WebCameraInputSystem.Utils
{
    public static class Alg
    {
        public static void Resize(Texture original, Texture2D target, bool flipY = true, bool mipmap = true, FilterMode filter = FilterMode.Bilinear)
        {
            var rt = RenderTexture.GetTemporary(target.width, target.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            RenderTexture.active = rt;
            if (flipY)
                Graphics.Blit(original, rt, new Vector2(-1, 1), new Vector2(1, 0));
            else
                Graphics.Blit(original, rt);
            target.filterMode = filter;
            target.ReadPixels(new Rect(0.0f, 0.0f, target.width, target.height), 0, 0);
            target.Apply();
            RenderTexture.ReleaseTemporary(rt);
        }

        public static float CalcDifference(float[] pixels, float[] background)
        {
            if (background == null) return 0;
            if (pixels.Length != background.Length) return 0;

            var difference = 0f;
            for (var i = 0; i < background.Length; i++)
                difference += MathF.Abs(MathF.Abs(background[i]) - MathF.Abs(pixels[i]));
            difference /= background.Length;
            return MathF.Round(difference, 3);
        }

        public static byte[] CropByBytes(byte[] bytes, Vector2Int textureSize, RectInt targetZone)
        {
            targetZone.ClampToBounds(new RectInt(0, 0, textureSize.x, textureSize.y));
            var result = new byte[targetZone.width * targetZone.height * 4];

            for (var lineIndex = 0; lineIndex < targetZone.height; lineIndex++)
            {
                var firstIndex = (textureSize.x * (targetZone.y + lineIndex) + targetZone.x) * 4;
                var lineLenght = targetZone.width * 4;
                for (int i = 0; i < lineLenght; i++)
                    result[i] = bytes[firstIndex + i];
            }
            return result;
        }

        public static float[] GetGrayScale(byte[] bytes)
        {
            var result = new float[bytes.Length / 4];
            var byteIndex = 0;
            for (var pixel = 0; pixel < result.Length; pixel++)
            {
                var rawValue =
                    (bytes[byteIndex + 0] * 0.3f) +
                    (bytes[byteIndex + 1] * 0.59f) +
                    (bytes[byteIndex + 2] * 0.11f);
                rawValue *= 1f / 255f;

                result[pixel] = MathF.Round(rawValue, 2);
                byteIndex += 4;
            }
            return result;
        }
    }
}
