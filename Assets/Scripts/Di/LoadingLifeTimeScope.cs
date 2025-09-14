using Services.SceneManagement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Di
{
    public class LoadingLifeTimeScope : LifetimeScope
    {
        [SerializeField] private SceneLoader _sceneLoader;
        
        protected override void Configure(IContainerBuilder builder)
        {
            LoadingScope(builder);
        }

        private void LoadingScope(IContainerBuilder builder)
        {
            builder.RegisterInstance<SceneLoader>(_sceneLoader);
        }
    }
}