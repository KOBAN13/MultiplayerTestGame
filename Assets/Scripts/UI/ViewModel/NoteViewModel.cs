using Helpers;
using R3;
using Services;
using UI.Base;
using UI.View;
using VContainer;

namespace UI.ViewModel
{
    public class NoteViewModel : Base.ViewModel
    {
        [Inject] private ScreenService _screenService;
        
        private readonly ViewModelBinder<string> _discriptionTextViewBinder = new (ViewModelBinderId.NOTE_DISCRIPTION);
        private readonly ViewModelBinder<string> _titleTextViewBinder = new (ViewModelBinderId.NOTE_TITLE);
        private readonly RefTypeViewModelBinder<ReactiveCommand> _buttonViewBinder = new (ViewModelBinderId.NOTE_BUTTON);
        
        public override void Initialize()
        {
            Bind(_discriptionTextViewBinder, _titleTextViewBinder, _buttonViewBinder);
            
            _buttonViewBinder.Value
                .Subscribe(_ => _screenService.CloseScreen<NoteScreen>())
                .AddTo(Disposable);
        }
        
        private void OnNoteDescriptionChanged(string description)
        {
            _discriptionTextViewBinder.Value = description;
        }
        
        private void OnNoteTitleChanged(string title)
        {
            _titleTextViewBinder.Value = title;
        }
    }
}