using CodeBase.MapLevel;
using CodeBase.StaticData.Audio;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace CodeBase.Services.Factory
{
    public interface IGameFactory : IService
    {
        UniTask<GameObject> CreatePlayer(Vector2 at);
        UniTask<GameObject> CreateFlySaw(Vector2 at, Vector3 scale, CancellationToken ct);
        UniTask CreateFX(Vector2 at, CancellationToken ct);
        UniTask CreateAudioPlayer(AudioConfigId id);
        UniTask<GameObject> CreatePlayerInLevelMap(MapLevelSlotContainer slotContainer, Vector2 at);
        UniTask<GameObject> CreateCmvCamera();
    }
}