using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.Windows
{
    [Serializable]
    public class WindowConfig
    {
        [SerializeField] private string _name = string.Empty;
        public WindowId WindowId;
        public BaseWindow Template;
        public AssetReferenceGameObject WindowReference;
    }
}