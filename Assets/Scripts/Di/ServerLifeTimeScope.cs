using Services;
using Sfs2X;
using VContainer;
using VContainer.Unity;

namespace Di
{
    public class ServerLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            ServerScope(builder);
        }
        
        private void ServerScope(IContainerBuilder builder)
        {
            var sfs = new SmartFox();
            
            builder
                .RegisterInstance(sfs)
                .AsImplementedInterfaces()
                .AsSelf();
            
            builder
                .Register<EncryptionService>(Lifetime.Singleton)
                .WithParameter("SFSTestGameKey568")
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}