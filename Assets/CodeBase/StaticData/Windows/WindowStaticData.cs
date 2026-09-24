using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Windows
{
    [CreateAssetMenu(menuName = "Static Data/Window static data", fileName = "WindowStaticData")]
    public class WindowStaticData : ScriptableObject
    {
        [field: SerializeField] public float CurtainSpeed { get; private set; } = 5;
        [field: SerializeField] public List<WindowConfig> Configs { get; private set; }
    }
}