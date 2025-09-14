using Helpers;
using UI.Base;
using UI.Binders;
using UI.ViewModel;
using UnityEngine;

namespace UI.View
{
    public class LoadingScreen : Screen<LoadingViewModel>
    {
        [SerializeField] private ProgressBarViewBinder progressBar = new(ViewModelBinderId.LOADING_BINDER);
        
        public override void Initialize()
        {
            Bind(progressBar);
        }
    }
}