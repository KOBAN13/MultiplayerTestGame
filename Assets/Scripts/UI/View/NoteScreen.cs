using Helpers;
using UI.Base;
using UI.Binders;
using UI.ViewModel;
using UnityEngine;

namespace UI.View
{
    public class NoteScreen : Screen<NoteViewModel>
    {
        [SerializeField] private TextViewBinder _discriptionTextViewBinder = new(ViewModelBinderId.NOTE_DISCRIPTION);
        [SerializeField] private TextViewBinder _titleTextViewBinder = new(ViewModelBinderId.NOTE_TITLE);
        [SerializeField] private ButtonViewBinder _buttonViewBinder = new(ViewModelBinderId.NOTE_BUTTON);
        
        public override void Initialize()
        {
            Bind(_discriptionTextViewBinder, _titleTextViewBinder, _buttonViewBinder);
        }
    }
}