using Helpers;
using R3;
using Services.Interface;
using UI.Base;
using UI.View;
using VContainer;

namespace UI.ViewModel
{
    public class LoginViewModel : Base.ViewModel
    {
        [Inject] private ILoginClientService _loginClientService;
        [Inject] private IScreenService _screenService;
        
        private readonly RefTypeViewModelBinder<ReactiveCommand<string>> _loginBinder = new(ViewModelBinderId.INPUT_LOGIN);
        private readonly RefTypeViewModelBinder<ReactiveCommand<string>> _passwordBinder = new(ViewModelBinderId.INPUT_PASSWORD);
        private readonly RefTypeViewModelBinder<ReactiveCommand> _signInBinder = new(ViewModelBinderId.BUTTON_LOGIN);
        private readonly RefTypeViewModelBinder<ReactiveCommand> _closeBinder = new(ViewModelBinderId.BUTTON_CLOSE);
        private readonly ViewModelBinder<string> _errorBinder = new(ViewModelBinderId.LOGIN_TEXT_ERROR);

        private string _login;
        private string _password;
        
        public override void Initialize()
        {
            Bind(_loginBinder, _passwordBinder);
            
            _signInBinder.Value.Subscribe(OnLoginRequest).AddTo(Disposable);
            
            _loginBinder.Value.Subscribe(OnLoginChanged).AddTo(Disposable);
            
            _passwordBinder.Value.Subscribe(OnPasswordChanged).AddTo(Disposable);
            
            _closeBinder.Value.Subscribe(OnCloseScreen).AddTo(Disposable);
            
            _loginClientService.LoginErrorRequest.Subscribe(OnLoginError).AddTo(Disposable);
        }
        
        private void OnLoginChanged(string login) => _login = login;
        
        private void OnPasswordChanged(string password) => _password = password;
        
        private void OnCloseScreen(Unit unit) => _screenService.CloseScreen<LoginScreen>();
        
        private void OnLoginError(string error) => _errorBinder.Value = error;

        private void OnLoginRequest(Unit unit)
        {
            if (string.IsNullOrEmpty(_login) || string.IsNullOrEmpty(_password)) 
                return;
            
            //TODO: Добавить выключение кнопки пока идет запрос, чтобы не было двойного нажатия
            
            _loginClientService.Login(_login, _password);
        }
    }
}