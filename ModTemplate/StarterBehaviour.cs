using OWML.ModHelper;
using OWML.ModHelper.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OuterWildsxASCII
{
    class ShaderApplier : ModBehaviour
    {
        RenderTexture cameraTexture;
        RenderTexture asciiTexture;
        ComputeShader computeShader;
        bool running = false;

        private void Start()
        {

            ModHelper.Console.WriteLine("Creating textures...");
            asciiTexture = new RenderTexture(1920, 1080, 0);
            asciiTexture.enableRandomWrite = true;
            asciiTexture.filterMode = FilterMode.Point;
            asciiTexture.Create();

            cameraTexture = new RenderTexture(1920, 1080, 0);
            cameraTexture.enableRandomWrite = true;
            cameraTexture.filterMode = FilterMode.Point;
            cameraTexture.Create();

            ModHelper.Console.WriteLine("Done!");

            ModHelper.Console.WriteLine("Loading bundle...");

            var shaderbundle = ModHelper.Assets.LoadBundle("asciishader");

            ModHelper.Console.WriteLine("Loading compute shader...");

            computeShader = shaderbundle.LoadAsset<ComputeShader>("ASCIIShader");

            ModHelper.Console.WriteLine("Done!");
            ModHelper.Console.WriteLine("Getting cameras...");
            ModHelper.Console.WriteLine("Instantiating texture cam");
            GameObject textureCameraGO = new GameObject();
            Camera camera = textureCameraGO.AddComponent<Camera>();
            ModHelper.Console.WriteLine("Setting parent");
            textureCameraGO.transform.SetParent(transform);
            ModHelper.Console.WriteLine("Setting target texture");
            camera.targetTexture = cameraTexture;
            running = true;
            ModHelper.Console.WriteLine("Done!");
        }


        void Update()
        {
            if (!running) { return; }
            computeShader.SetTexture(0, "cameraTexture", cameraTexture);
            computeShader.SetTexture(0, "asciiTexture", asciiTexture);
            computeShader.SetFloat("scaleFactor", 1);
            computeShader.Dispatch(0, 1920 / (8 * 8), 1080 / (8 * 8), 1);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!running) { return; }
            Graphics.Blit(asciiTexture, destination);
        }
    
    }
}
