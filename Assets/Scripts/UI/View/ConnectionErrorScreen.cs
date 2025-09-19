using Helpers;
using UI.Base;
using UI.Binders;
using UI.ViewModel;
using UnityEngine;

namespace UI.View
{
    public class ConnectionErrorScreen : Screen<ConnectionErrorViewModel>
    {
        [SerializeField] private TextViewBinder _textViewBinder = new(ViewModelBinderId.CONNECTION_ERROR_DISCRIPTION);
        [SerializeField] private ButtonViewBinder _buttonViewBinder = new(ViewModelBinderId.CONNECTION_ERROR_BUTTON);
        
        public override void Initialize()
        {
            Bind(_textViewBinder, _buttonViewBinder);
        }
    }
}