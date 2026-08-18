using UnityEngine;

namespace CodeBase.MapLevel
{
    public class MapLevelComponentContainer : MonoBehaviour
    {
        [field: SerializeField] public MapLevelSlotContainer SlotContainer { get; private set; }
    }
}