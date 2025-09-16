using System.Linq;
using Factories;
using Services;
using Services.Interface;
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
            Register<ViewModelFactory>();
            Register<ViewsFactory>();
            Register<ScreensFactory>();
        }

        private void RegisterServices()
        {
            Register<SceneResources>();
            Register<SceneService>();
            RegisterInstance(_sceneLoader);
            Register<ScreenService>();
        }
    }
}