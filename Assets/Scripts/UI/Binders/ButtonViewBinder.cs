using System;
using R3;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Binders
{
    [Serializable]
    public class ButtonViewBinder : ViewBinder<ReactiveCommand>
    {
        [SerializeField] private Button _button;
        private ReactiveCommand _reactiveCommand;
        
        public ButtonViewBinder(string id) : base(id)
        {
        }

        public override void Parse(ReactiveCommand value)
        {
            _reactiveCommand = value;
            _button.onClick.AddListener(OnClicked);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _button.onClick.RemoveListener(OnClicked);
            _reactiveCommand = null;
        }

        private void OnClicked()
        {
            _reactiveCommand.Execute(Unit.Default);
        }
    }
}