using Factories;
using Installer;
using Services;
using Services.Connections;
using Sfs2X;
using UnityEngine;
using VContainer;

namespace Di
{
    public class ServerLifeTimeScope : BaseLifeTimeScope
    {
        [SerializeField] private MainMenuInstaller _mainMenuInstaller;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Builder = builder;
            
            RegisterFactories();
            RegisterServices();
            ServerScope();
        }
        
        private void RegisterFactories()
        {
            Register<ViewModelFactory>(Lifetime.Singleton);
            Register<ViewsFactory>(Lifetime.Singleton);
            Register<ScreensFactory>(Lifetime.Singleton);
        }

        private void RegisterServices()
        {
            Register<ScreenService>(Lifetime.Singleton);
        }
        
        private void ServerScope()
        {
            var sfs = new SmartFox();
            RegisterInstance(_mainMenuInstaller);
            RegisterInstance(sfs);
            RegisterWithArgument<EncryptionService, string>(Lifetime.Singleton, "SFSTestGameKey568");
            Register<ConnectionService>(Lifetime.Singleton);
            Register<LoginClientService>(Lifetime.Singleton);
        }
    }
}