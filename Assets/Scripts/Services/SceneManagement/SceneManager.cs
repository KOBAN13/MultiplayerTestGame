using System;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using SceneManagment;
using Services.Interface;
using Services.SceneManagement.Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public class ScreenService : IScenesService
    {
        private SceneResources _resources;
        private AsyncOperationHandle<SceneInstance> _previousScene;
        public delegate UniTask<bool> LoadSceneDelegate(string typeScene);
        public event Action<LoadSceneDelegate, string> OnLoadScene;
        public event Action SceneIsLoad;
        
        private bool _isLoadScene = false;

        public void Construct(SceneResources resources)
        {
            _resources = resources;
        }
        
        public async UniTask LoadScene(SceneGroup sceneGroup, IProgress<float> progress, TypeScene typeScene)
        {
            await UnloadScene();

            var scene = sceneGroup.FindSceneByReference(typeScene);

            if (scene.State == SceneReferenceState.Addressable)
            {
                LoadSceneDelegate loadSceneDelegate = async (sceneType) => _isLoadScene = await LoadSceneFromBundle(sceneType, progress);
                OnLoadScene?.Invoke(loadSceneDelegate, scene.Path);
            }

            await UniTask.WaitUntil(() => _isLoadScene);
            SceneIsLoad?.Invoke();
            _isLoadScene = false;
        }

        private async UniTask UnloadScene()
        {
            if(_previousScene.IsValid() == false) return;
            
            await Addressables.UnloadSceneAsync(_previousScene);

            foreach (var resource in _resources.ObjectToRelease)
            {
                resource.Invoke();
            }
            
            _resources.ClearObject();

            await Resources.UnloadUnusedAssets();
        }
        
        private async UniTask<bool> LoadSceneFromBundle(string sceneName, IProgress<float> progress)
        {
            AsyncOperationHandle<SceneInstance> sceneLoadOperation = Addressables.LoadSceneAsync(sceneName);
            _previousScene = sceneLoadOperation;
            
            while (!sceneLoadOperation.IsDone)
            {
                progress.Report(sceneLoadOperation.PercentComplete);
                await UniTask.WaitForSeconds(0.1f);
            }

            if (sceneLoadOperation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Failed to load scene: " + sceneName);
            }

            var sceneInstance = sceneLoadOperation.Result;
            if (sceneInstance.Scene.IsValid())
            {
                var loadedScene = SceneManager.GetSceneByPath(sceneInstance.Scene.path);

                if (loadedScene.IsValid())
                    SceneManager.SetActiveScene(loadedScene);
            }
            else
                Debug.LogWarning("Invalid scene instance loaded.");

            await UniTask.Yield();
            return true;
        }
    }
}