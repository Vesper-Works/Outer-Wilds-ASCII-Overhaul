using OWML.ModHelper;
using OWML.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OuterWildsxASCII
{
    class ASCIIRendering : MonoBehaviour
    {
        RenderTexture cameraTexture;
        RenderTexture asciiTexture;
        ComputeShader computeShader;
        bool running;
        float scaleFactor
        {
            get
            {
                return float.Parse(StarterBehaviour.Instance.ModHelper.Config.Settings["ASCII scale"].ToString());
            }
        }
        float backBrightness
        {
            get
            {
                return float.Parse(StarterBehaviour.Instance.ModHelper.Config.Settings["Brightness behind ASCII (0 - 1)"].ToString());
            }
        }
        public void Ready(RenderTexture cameraTexture, RenderTexture asciiTexture, ComputeShader computeShader)
        {
            this.cameraTexture = cameraTexture;
            this.asciiTexture = asciiTexture;
            this.computeShader = computeShader;
            running = true;
        }
        private void Update()
        {
            if (!running || Keyboard.current["h"].IsPressed()) { return; }
            computeShader.SetTexture(0, "cameraTexture", cameraTexture);
            computeShader.SetTexture(0, "asciiTexture", asciiTexture);
            computeShader.SetFloat("scaleFactor", scaleFactor);
            computeShader.SetFloat("backBrightness", backBrightness);
            computeShader.Dispatch(0, 1920 / (8 * 8), 1080 / (8 * 8), 1);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!running)
            {
                return;
            }
            if (Keyboard.current["h"].IsPressed())
            {
                Graphics.Blit(cameraTexture, destination);
            }
            else
            {
                Graphics.Blit(asciiTexture, destination);
            }

        }
    }
}
