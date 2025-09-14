using System.Collections.Generic;
using Sirenix.OdinInspector;
using UI.Base;
using UnityEngine;
using Utils;

namespace Configs
{
    [CreateAssetMenu(fileName = "ScreensConfig", menuName = "Configs/ScreensConfig")]
    public class ScreensConfig : SerializedScriptableObject
    {
        [field: SerializeField] public Transform Root { get; private set; }
        
        [SerializeField] private List<AddressablePrefabByType<View>> _screens;

        public IReadOnlyList<AddressablePrefabByType<View>> Screens => _screens;
    }
}