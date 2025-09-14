using Services.SceneManagement;
using Services.SceneManagement.Enums;
using UnityEngine;
using VContainer;

namespace Installer
{
    public class EntryPointInstaller : MonoBehaviour
    {
        [Inject] private SceneLoader _sceneLoader;

        private async void Awake()
        {
            await _sceneLoader.LoadScene(TypeScene.MainMenu);
        }
    }
}