using UnityEngine;

namespace WebCameraInputSystem.Tools
{
    public static class TextureScaler
    {
        [System.Obsolete]
        public static Texture2D ScaledCopy(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
        {
            var texR = new Rect(0, 0, width, height);
            GpuScale(src, width, height, mode);

            //Get rendered data back to a new texture
            var result = new Texture2D(width, height, TextureFormat.ARGB32, true);
            result.Resize(width, height);
            result.ReadPixels(texR, 0, 0, true);
            return result;
        }

        [System.Obsolete]
        public static void Scale(Texture2D tex, int width, int height, FilterMode mode = FilterMode.Trilinear)
        {
            var texR = new Rect(0, 0, width, height);
            GpuScale(tex, width, height, mode);

            // Update new texture
            tex.Resize(width, height);
            tex.ReadPixels(texR, 0, 0, true);
            tex.Apply(true);    //Remove this if you hate us applying textures for you :)
        }

        static void GpuScale(Texture2D src, int width, int height, FilterMode fmode)
        {
            //We need the source texture in VRAM because we render with it
            src.filterMode = fmode;
            src.Apply(true);

            //Using RTT for best quality and performance. Thanks, Unity 5
            var rtt = new RenderTexture(width, height, 32);

            //Set the RTT in order to render to it
            Graphics.SetRenderTarget(rtt);

            //Setup 2D matrix in range 0..1, so nobody needs to care about sized
            GL.LoadPixelMatrix(0, 1, 1, 0);

            //Then clear & draw the texture to fill the entire RTT.
            GL.Clear(true, true, new Color(0, 0, 0, 0));
            Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
        }
    }
}
