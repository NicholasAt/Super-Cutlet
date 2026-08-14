using CodeBase.MapLevel;
using CodeBase.StaticData.Audio;
using UnityEngine;

namespace CodeBase.Services.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreatePlayer(Vector2 at);

        GameObject CreateCmvCamera();

        void CreateAudioPlayer(AudioConfigId id);

        GameObject CreatePlayerInLevelMap(MapLevelSlotContainer slotContainer, Vector2 at);

        GameObject CreateFlySaw(Vector2 at, Vector3 scale);
        void CreateFx(Vector2 at);
    }
}