﻿using OWML.ModHelper;
using UnityEngine;
using UnityEngine.InputSystem;
using OWML.ModHelper.Events;

namespace OuterWildsxASCII
{
    public class StarterBehaviour : ModBehaviour
    {
        RenderTexture cameraTexture;
        RenderTexture asciiTexture;
        ComputeShader computeShader;
        Camera ASCIICamera;
        Camera NormalCamera { get => Camera.main; }
        public static StarterBehaviour Instance { get; set; }
        private void Start()
        {
            Instance = this;
            //ModHelper.Console.WriteLine("Skipping splash screen...");
            //var titleScreenAnimation = FindObjectOfType<TitleScreenAnimation>();
            //titleScreenAnimation.SetValue("_fadeDuration", 0);
            //titleScreenAnimation.SetValue("_gamepadSplash", false);
            //titleScreenAnimation.SetValue("_introPan", false);
            //titleScreenAnimation.Invoke("FadeInTitleLogo");
            //ModHelper.Console.WriteLine("Done!");

            ModHelper.Events.Scenes.OnCompleteSceneChange += OnCompleteSceneChange;

            Textures();
            ComputeShader();
            CameraWork();
        }

        private void OnCompleteSceneChange(OWScene oldScene, OWScene newScene)
        {
            CameraWork();
        }

        private void Textures()
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
        }

        private void ComputeShader()
        {
            ModHelper.Console.WriteLine("Loading compute shader...");

            var shaderbundle = ModHelper.Assets.LoadBundle("asciishader");

            computeShader = shaderbundle.LoadAsset<ComputeShader>("ASCIIShader");
            ModHelper.Console.WriteLine("Done!");
        }

        private void CameraWork()
        {
            if (NormalCamera == null) { return; }

            GameObject textureCameraGO = new GameObject();
            ASCIICamera = textureCameraGO.AddComponent<Camera>();
            textureCameraGO.AddComponent<OWCamera>();

            textureCameraGO.transform.SetParent(NormalCamera.transform);
            textureCameraGO.transform.position = Vector3.zero;
            textureCameraGO.transform.rotation = Quaternion.identity;
            ASCIICamera.CopyFrom(NormalCamera);
            ASCIICamera.cullingMask = 1 << 5;
            NormalCamera.targetTexture = cameraTexture;
            ModHelper.Console.WriteLine("Done!");

            AddACIIRenderingToCamera(textureCameraGO);

        }
        private void AddACIIRenderingToCamera(GameObject camera)
        {
            camera.AddComponent<ASCIIRendering>().Ready(cameraTexture, asciiTexture, computeShader);
        }
    }
}