using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Asset reference", fileName = "AssetReferenceStaticData")]
    public class AssetReferenceStaticData : ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject FlySawReference { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject FxReference { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject AudioPlayerReference { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject MapLevelPlayerReference { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject CMVcamReference { get; private set; }
    }
}