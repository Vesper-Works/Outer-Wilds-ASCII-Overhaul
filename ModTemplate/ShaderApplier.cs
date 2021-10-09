using OWML.ModHelper;
using OWML.Common;
using UnityEngine;

namespace OuterWildsxASCII
{
    public class ShaderApplier : MonoBehaviour
    {
        RenderTexture cameraTexture;
        RenderTexture asciiTexture;
        ComputeShader computeShader;
        new IModHelper ModHelper { get => StarterBehaviour.Instance.ModHelper; }

        private void Start()
        {
            ModHelper.Console.WriteLine(this.ToString());
            Textures();
            ComputeShader();
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
            ModHelper.Console.WriteLine("Getting cameras...");
            //ModHelper.Console.WriteLine("Instantiating texture cam");
            GameObject textureCameraGO = new GameObject();
            Camera textureCamera = textureCameraGO.AddComponent<Camera>();
            textureCameraGO.AddComponent<OWCamera>();
        
            textureCameraGO.transform.SetParent(transform);
            textureCameraGO.transform.position = Vector3.zero;
            textureCameraGO.transform.rotation = Quaternion.identity;
            textureCamera.CopyFrom(GetComponent<Camera>());

            GetComponent<Camera>().targetTexture = cameraTexture;
            ModHelper.Console.WriteLine("Done!");

            AddACIIRenderingToCamera(textureCameraGO);

        }
        private void AddACIIRenderingToCamera(GameObject camera)
        {
            camera.AddComponent<ASCIIRendering>().Ready(cameraTexture, asciiTexture, computeShader);
        }
        OWCamera CopyOWCamera(OWCamera original, GameObject destination)
        {
            OWCamera copy = destination.AddComponent<OWCamera>();
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = typeof(OWCamera).GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            System.Reflection.PropertyInfo[] properties = typeof(OWCamera).GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (!property.CanWrite) { continue; }
                property.SetValue(copy, property.GetValue(original, null), null);
            }
            return copy;
        }
  
    
    }
}
