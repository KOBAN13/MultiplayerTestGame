using Helpers;
using Services.SceneManagement;
using UI.Base;
using R3;
using VContainer;

namespace UI.ViewModel
{
    public class LoadingViewModel : Base.ViewModel
    {
        [Inject] private SceneLoader _sceneLoader;
        private readonly ViewModelBinder<float> _progressBinder = new(ViewModelBinderId.LOADING_BINDER);
        
        public override void Initialize()
        {
            Bind(_progressBinder);
            
            _sceneLoader.FillAmount
                .Subscribe(value => _progressBinder.Value = value)
                .AddTo(Disposable);
        }
    }
}