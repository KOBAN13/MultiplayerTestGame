using VContainer;
using VContainer.Unity;

namespace Di
{
    public class BaseLifeTimeScope : LifetimeScope
    {
        protected IContainerBuilder Builder;
        
        protected void Register<T>() where T : class
        {
            Builder.Register<T>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
        
        protected void RegisterInstance<T>(T instance) where T : class
        {
            Builder.RegisterInstance(instance).AsImplementedInterfaces().AsSelf();
        }
    }
}