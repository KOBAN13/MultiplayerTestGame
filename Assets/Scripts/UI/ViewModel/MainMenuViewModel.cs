using Helpers;
using R3;
using Services;
using UI.Base;
using UI.View;
using VContainer;

namespace UI.ViewModel
{
    public class MainMenuViewModel : Base.ViewModel
    {
        [Inject] private ScreenService _screenService;
        
        private readonly RefTypeViewModelBinder<ReactiveCommand> _signInButtonViewBinder = new (ViewModelBinderId.SIGN_IN_BUTTON);
        private readonly RefTypeViewModelBinder<ReactiveCommand> _signUpButtonViewBinder = new (ViewModelBinderId.SIGN_UP_BUTTON);
        private readonly RefTypeViewModelBinder<ReactiveCommand> _playButtonViewBinder = new (ViewModelBinderId.PLAY_BUTTON);
        
        public override void Initialize()
        {
            Bind(_signInButtonViewBinder, _signUpButtonViewBinder, _playButtonViewBinder);
            
            _signUpButtonViewBinder.Value.Subscribe(SignUp).AddTo(Disposable);
            _signInButtonViewBinder.Value.Subscribe(SignIn).AddTo(Disposable);
            _playButtonViewBinder.Value.Subscribe(Play).AddTo(Disposable);
        }
        
        private async void SignUp(Unit unit) => await _screenService.OpenAsync<SignUpScreen>();
        private async void SignIn(Unit unit) => await _screenService.OpenAsync<LoginScreen>();

        private async void Play(Unit unit)
        {
            await _screenService.OpenAsync<NoteScreen>();
        }
    }
}