using Helpers;
using UI.Base;
using UI.Binders;
using UI.ViewModel;
using UnityEngine;

namespace UI.View
{
    public class MainMenuScreen : Screen<MainMenuViewModel>
    {
        [SerializeField] private ButtonViewBinder _signUpButton = new(ViewModelBinderId.SIGN_UP_BUTTON);
        [SerializeField] private ButtonViewBinder _signInButton = new(ViewModelBinderId.SIGN_IN_BUTTON);
        [SerializeField] private ButtonViewBinder _playButton = new(ViewModelBinderId.PLAY_BUTTON);
        
        public override void Initialize()
        {
            Bind(_signUpButton, _signInButton, _playButton);
        }
    }
}