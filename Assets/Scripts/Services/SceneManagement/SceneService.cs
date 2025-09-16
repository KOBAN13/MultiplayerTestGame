using System;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using Services.Interface;
using Services.SceneManagement.Enums;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public class SceneService : IScenesService
    {
        private SceneResources _resources;
        private AsyncOperationHandle<SceneInstance> _previousScene;
        private readonly Subject<Unit> _sceneIsLoadSubject = new();
        public Observable<Unit> SceneIsLoad => _sceneIsLoadSubject;
        

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
                await LoadSceneFromBundle(scene.Path, progress)
                    .ContinueWith(_ => _sceneIsLoadSubject.OnNext(Unit.Default));
            }
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