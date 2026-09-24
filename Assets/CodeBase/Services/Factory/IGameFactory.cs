using CodeBase.MapLevel;
using CodeBase.StaticData.Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateCmvCamera();

        void CreateAudioPlayer(AudioConfigId id);

        GameObject CreatePlayerInLevelMap(MapLevelSlotContainer slotContainer, Vector2 at);

        GameObject CreateFlySaw(Vector2 at, Vector3 scale);
        void CreateFx(Vector2 at);
        UniTask<GameObject> CreatePlayer(Vector2 at);
    }
}