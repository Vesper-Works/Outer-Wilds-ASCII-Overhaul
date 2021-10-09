using OWML.ModHelper;
using UnityEngine;
using OWML.ModHelper.Events;

namespace OuterWildsxASCII
{
    public class StarterBehaviour : ModBehaviour
    {
        public static StarterBehaviour Instance { get; set; }
        private void Start()
        {
            ModHelper.Console.WriteLine(this.ToString());
            Instance = this;
            ModHelper.Console.WriteLine("Skipping splash screen...");
            var titleScreenAnimation = FindObjectOfType<TitleScreenAnimation>();
            titleScreenAnimation.SetValue("_fadeDuration", 0);
            titleScreenAnimation.SetValue("_gamepadSplash", false);
            titleScreenAnimation.SetValue("_introPan", false);
            titleScreenAnimation.Invoke("FadeInTitleLogo");
            ModHelper.Console.WriteLine("Done!");

            ModHelper.Events.Scenes.OnCompleteSceneChange += OnCompleteSceneChange;

        }
        private void OnCompleteSceneChange(OWScene oldScene, OWScene newScene)
        {
            if (newScene != OWScene.SolarSystem) { return; }
            Camera.main.gameObject.AddComponent<ShaderApplier>();
        }
    }
}