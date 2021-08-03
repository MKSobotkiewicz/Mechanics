using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Utility
{
    public static class Camera
    {
        public static Texture2D RenderToTexture(UnityEngine.Camera camera)
        {
            var rt = new RenderTexture(camera.pixelWidth,camera.pixelHeight,32);
            camera.targetTexture = rt;
            RenderTexture.active = camera.targetTexture;
            var texture = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
            camera.Render();
            texture.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
            texture.Apply();
            return texture;
        }
    }
}
