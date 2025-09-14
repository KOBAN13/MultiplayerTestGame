using VContainer;

namespace Di
{
    public class EntryPointLifetimeScope : BaseLifeTimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Builder = builder;
        }
    }
}