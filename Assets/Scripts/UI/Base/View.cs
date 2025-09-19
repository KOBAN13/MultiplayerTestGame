﻿using System;
using Factories;
using UnityEngine;
using VContainer;

namespace UI.Base
{
    public abstract class View<TViewModel> : View
        where TViewModel : ViewModel, new()
    {
        [Inject] private ViewModelFactory _viewModelFactory;

        private ViewBinder[] _viewBinders;
        public TViewModel ViewModel { get; private set; }
        
        protected void Bind( params ViewBinder[] viewBinders)
        {
            ViewModel = _viewModelFactory.Create<TViewModel>(viewBinders);
            foreach (var viewBinder in viewBinders)
            {
                viewBinder.Initialize();
            }
        }

        public override void Dispose()
        {
            foreach (var viewBinder in _viewBinders)
            {
                viewBinder.Dispose();
            }
        }
    }

    public abstract class View : MonoBehaviour, IDisposable
    {
        public abstract void Initialize();

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        public virtual void Dispose()
        {
        }
    }
}