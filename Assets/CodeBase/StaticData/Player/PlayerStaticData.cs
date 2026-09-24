using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.Player
{
    [CreateAssetMenu(fileName = "New Player Data", menuName = "Static Data/PlayerData")]
    public class PlayerStaticData : ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject PlayerReference { get; private set; }
        [field: SerializeField] public PlayerGroundMoveConfig GroundMoveConfig { get; private set; }
        [field: SerializeField] public PlayerClimbMoveConfig ClimbMoveConfig { get; private set; }
        [field: SerializeField] public float GravityScale { get; private set; }
        [field: SerializeField] public float RestartTimeAfterPlayerDeath { get; private set; }

    }
}