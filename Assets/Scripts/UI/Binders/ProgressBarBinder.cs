﻿using System;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Binders
{
    [Serializable]
    public class ProgressBarViewBinder : ViewBinder<float>
    {
        [SerializeField] private Image _image;
        
        public ProgressBarViewBinder(string id) : base(id)
        {
            
        }

        public override void Parse(float value)
        {
            _image.fillAmount = value;
        }
    }
}