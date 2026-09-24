using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.Windows
{
    [CreateAssetMenu(menuName = "Static Data/Window static data", fileName = "WindowStaticData")]
    public class WindowStaticData : ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject UIRootReference { get; private set; }
        [field: SerializeField] public List<WindowConfig> Configs { get; private set; }
    }
}