using UI.Base;
using VContainer;

namespace UI.Factory
{
    public class ViewModelFactory : GameCore.Factories.Factory
    {
        [Inject] private IObjectResolver _objectResolver;
        
        public T Create<T>(params ViewBinder[] viewBinders) where T : Base.ViewModel, new()
        {
            var viewModel = new T();
            _objectResolver.Inject(viewModel);
            viewModel.Initialize();
            
            foreach (var viewBinder in viewBinders)
            {
                if (viewModel.ViewModelBinders.TryGetValue(viewBinder.Id, out var viewModelBinder))
                {
                    viewBinder.ViewModelBinder = viewModelBinder;
                }
            }
            
            return viewModel;
        }
    }
}