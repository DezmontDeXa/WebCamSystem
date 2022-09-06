using UnityEngine;
using System.Linq;
using System;

namespace WebCameraInputSystemOLD.Utils
{
    public static class AlgOld
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

        public static Texture ResizeCopy(Texture original, RectInt size, bool flipY = true, bool mipmap = true, FilterMode filter = FilterMode.Bilinear)
        {
            var rt = RenderTexture.GetTemporary(size.width, size.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            RenderTexture.active = rt;
            if (flipY)
                Graphics.Blit(original, rt, new Vector2(-1, 1), new Vector2(1, 0));
            else
                Graphics.Blit(original, rt);
            var target = new Texture2D(size.width, size.height);
            target.filterMode = filter;
            target.ReadPixels(new Rect(0.0f, 0.0f, size.width, size.height), 0, 0);
            target.Apply();
            RenderTexture.ReleaseTemporary(rt);
            return target;
        }

        public static Texture2D FlipY(Texture origin, FilterMode filter = FilterMode.Point)
        {
            var rt = RenderTexture.GetTemporary(origin.width, origin.height, 32);

            Graphics.Blit(origin, rt, new Vector2(-1, 1), new Vector2(1, 0));

            var result = new Texture2D(origin.width, origin.height);

            result.filterMode = filter;
            result.ReadPixels(new Rect(0.0f, 0.0f, origin.width, origin.height), 0, 0);
            result.Apply();

            RenderTexture.ReleaseTemporary(rt);

            return result;
        }

        public static float[,] CalcDifferenceMatrix(float[,] grayscaled, float[,] background)
        {
            if (background == null) return default;
            if (grayscaled.Length != background.Length) return default;

            var result = new float[grayscaled.GetLength(0), grayscaled.GetLength(1)];

            for (var x = 0; x < grayscaled.GetLength(0); x++)
            {
                for (var y = 0; y < grayscaled.GetLength(1); y++)
                {
                    result[x, y] =
                        MathF.Round(
                            MathF.Abs(
                                MathF.Abs(background[x, y]) - MathF.Abs(grayscaled[x, y])));
                }
            }
            return result;
        }

        public static void Crop(Texture origin, ref Texture2D target, RectInt _rect)
        {
            Graphics.CopyTexture(origin, 0, 0, _rect.x, _rect.y, _rect.width, _rect.height, target, 0, 0, 0, 0);
            target.Apply();
        }

        public static void CropByBlit(Texture origin, ref Texture2D target, RectInt _rect)
        {
            var rt = RenderTexture.GetTemporary(_rect.width, _rect.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            RenderTexture.active = rt;
            Graphics.Blit(origin, rt, new Vector2(1, 1), new Vector2(0, 0));
            target.ReadPixels(new Rect(0.0f, 0.0f, _rect.width, _rect.height), 0, 0);
            target.Apply();
            RenderTexture.ReleaseTemporary(rt);
        }

        public static float[,] GetGrayScaleMatrix(byte[] bytes, Vector2Int size)
        {
            var result = new float[size.x, size.y];
            var byteIndex = 0;
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    var value =
                    (bytes[byteIndex + 0] * 0.3f) +
                    (bytes[byteIndex + 1] * 0.59f) +
                    (bytes[byteIndex + 2] * 0.11f);
                    value *= 1f / 255f;
                    result[x, y] = MathF.Round(value, 2);
                    byteIndex += 4;
                }
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

        public static float[] GetGrayScale(byte[] bytes)
        {
            var result = new float[bytes.Length / 4];
            var byteIndex = 0;
            for (var pixel = 0; pixel < result.Length; pixel++)
            {
                var rawValue =
                    (bytes[byteIndex + 0] * 0.21f) +
                    (bytes[byteIndex + 1] * 0.72f) +
                    (bytes[byteIndex + 2] * 0.07f);
                rawValue /= 255f;

                result[pixel] = MathF.Round(rawValue, 2);
                byteIndex += 4;
            }
            return result;
        }

        public static float SquareMediateValue(params float[] values)
        {
            return MathF.Sqrt(values.Sum(x => MathF.Pow(x, 2)) / values.Length);
        }

        public static float MediateValue(params float[] values)
        {
            return values.Sum() / values.Length;
        }
    }
}
