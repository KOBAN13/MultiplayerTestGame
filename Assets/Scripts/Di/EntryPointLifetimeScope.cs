﻿using Installer;
using UnityEngine;
using VContainer;

namespace Di
{
    public class EntryPointLifetimeScope : BaseLifeTimeScope
    {
        [SerializeField] private EntryPointInstaller _installer; 
        protected override void Configure(IContainerBuilder builder)
        {
            Builder = builder;
            
            BuildEntryPoint(builder);
        }

        private void BuildEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterInstance(_installer).AsImplementedInterfaces().AsSelf();
        }
    }
}