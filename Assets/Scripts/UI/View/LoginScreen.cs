using Helpers;
using UI.Base;
using UI.Binders;
using UI.ViewModel;
using UnityEngine;

namespace UI.View
{
    public class LoginScreen : Screen<LoginViewModel>
    {
        [SerializeField] private InputFieldSetTextViewBinder _loginBinder = new(ViewModelBinderId.INPUT_LOGIN);
        [SerializeField] private InputFieldSetTextViewBinder _passwordBinder = new(ViewModelBinderId.INPUT_PASSWORD);
        [SerializeField] private TextViewBinder _errorBinder = new(ViewModelBinderId.LOGIN_TEXT_ERROR);
        [SerializeField] private ButtonViewBinder _signInBinder = new(ViewModelBinderId.BUTTON_LOGIN);
        [SerializeField] private ButtonViewBinder _closeBinder = new(ViewModelBinderId.BUTTON_CLOSE);
        
        public override void Initialize()
        {
            Bind(_loginBinder, _passwordBinder, _signInBinder, _closeBinder, _errorBinder);
        }
    }
}