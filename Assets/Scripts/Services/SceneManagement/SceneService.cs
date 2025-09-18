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
        private SceneLoader _sceneLoader;
        private AsyncOperationHandle<SceneInstance> _previousScene;
        private readonly Subject<Unit> _sceneIsLoadSubject = new();
        public Observable<Unit> SceneIsLoad => _sceneIsLoadSubject;
        

        public void Construct(SceneLoader sceneLoader, SceneResources resources)
        {
            _resources = resources;
            _sceneLoader = sceneLoader;
        }
        
        public async UniTask LoadScene(SceneGroup sceneGroup, IProgress<float> progress, TypeScene typeScene)
        {
            await UnloadScene();

            var scene = sceneGroup.FindSceneByReference(typeScene);
            
            if (scene.State == SceneReferenceState.Addressable)
            {
                await LoadSceneFromBundle(scene.Path, sceneGroup, progress);
            }
        }

        private async UniTask UnloadScene()
        {
            if(_previousScene.IsValid() == false) 
                return;
            
            await Addressables.UnloadSceneAsync(_previousScene);

            foreach (var resource in _resources.ObjectToRelease)
            {
                resource.Invoke();
            }
            
            _resources.ClearObject();

            await Resources.UnloadUnusedAssets();
        }

        private async void UnloadInitialScene(TypeScene typeScene, Scene initialScene, SceneGroup sceneGroup)
        {
            var scene = sceneGroup.FindSceneByReference(typeScene);
            
            if (initialScene.isLoaded && scene.Name == initialScene.name)
            {
                await SceneManager.UnloadSceneAsync(initialScene);
            }
        }

        private async UniTask<bool> LoadSceneFromBundle(
            string sceneName,
            SceneGroup sceneGroup,
            IProgress<float> progress
        )
        {
            var previousScene = SceneManager.GetActiveScene();
            
            var sceneLoadOperation = Addressables.LoadSceneAsync(
                sceneName,
                LoadSceneMode.Additive,
                false
            );
            
            _previousScene = sceneLoadOperation;

            var fakeProgress = 0f;
            
            while (_sceneLoader.Progress.Value < 1 || !sceneLoadOperation.IsDone)
            {
                var target = Mathf.Clamp01(sceneLoadOperation.PercentComplete);
                fakeProgress = Mathf.MoveTowards(fakeProgress, target, 0.01f);
                progress.Report(fakeProgress);

                await UniTask.Yield();
            }
            
            if (sceneLoadOperation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Failed to load scene: " + sceneName);
                return false;
            }

            var sceneInstance = sceneLoadOperation.Result;
            if (!sceneInstance.Scene.IsValid())
            {
                Debug.LogWarning("Invalid scene instance loaded.");
                return false;
            }
            
            await sceneInstance.ActivateAsync();
            
            SceneManager.SetActiveScene(sceneInstance.Scene);

            UnloadInitialScene(
                TypeScene.InitialScene,
                previousScene,
                sceneGroup);
            
            _sceneLoader.IsLoading.Value = false;
            _sceneIsLoadSubject.OnNext(Unit.Default);

            return true;
        }
    }
}