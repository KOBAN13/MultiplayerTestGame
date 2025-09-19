using System.Linq;
using Factories;
using Services;
using Services.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Di
{
    public class RootLifeTimeScope : BaseLifeTimeScope
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private AssetLabelReference _configLable;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Builder = builder;
            
            RegisterConfigs();
            RegisterFactories();
            RegisterServices();
        }

        private void RegisterConfigs()
        {
            var configs = Addressables
                .LoadAssetsAsync<ScriptableObject>(_configLable, null)
                .WaitForCompletion()
                .ToList();

            foreach (var config in configs)
            {
                RegisterInstance(config);
            }
        }

        private void RegisterFactories()
        {
            Register<ViewModelFactory>(Lifetime.Singleton);
            Register<ViewsFactory>(Lifetime.Singleton);
            Register<ScreensFactory>(Lifetime.Singleton);
        }

        private void RegisterServices()
        {
            Register<SceneResources>(Lifetime.Singleton);
            Register<SceneService>(Lifetime.Singleton);
            RegisterInstance(_sceneLoader);
            Register<ScreenService>(Lifetime.Singleton);
        }
    }
}