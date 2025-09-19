using Helpers;
using R3;
using Services;
using Services.Interface;
using UI.Base;
using UI.View;
using VContainer;

namespace UI.ViewModel
{
    public class ConnectionErrorViewModel : Base.ViewModel
    {
        [Inject] private IConnectionService _connectionService;
        [Inject] private ScreenService _screenService;
        
        private readonly ViewModelBinder<string> _textViewBinder = new (ViewModelBinderId.CONNECTION_ERROR_DISCRIPTION);
        private readonly RefTypeViewModelBinder<ReactiveCommand> _buttonViewBinder = new (ViewModelBinderId.CONNECTION_ERROR_BUTTON);


        public override void Initialize()
        {
            Bind(_textViewBinder, _buttonViewBinder);
            
            _buttonViewBinder.Value
                .Subscribe(_ => _screenService.CloseScreen<ConnectionErrorScreen>())
                .AddTo(Disposable);
            
            _connectionService.ConnectionErrorDescription
                .Subscribe(OnConnectionErrorDescriptionChanged)
                .AddTo(Disposable);
        }
        
        private void OnConnectionErrorDescriptionChanged(string description)
        {
            _textViewBinder.Value = "Error description: " + description;
        }
    }
}