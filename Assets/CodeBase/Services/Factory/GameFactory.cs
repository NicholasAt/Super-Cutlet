using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic;
using CodeBase.MapLevel;
using CodeBase.Player;
using CodeBase.Player.PlayerMove;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ReloadScene;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Audio;
using UnityEngine;

namespace CodeBase.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticDataService;
        private readonly IReloadSceneService _reloadScene;
        private readonly IPersistentProgressService _persistentProgressService;

        public GameFactory(IAssetProvider assetProvider, IInputService inputService, IStaticDataService staticDataService, IReloadSceneService reloadScene, IPersistentProgressService persistentProgressService)
        {
            _assetProvider = assetProvider;
            _inputService = inputService;
            _staticDataService = staticDataService;
            _reloadScene = reloadScene;
            _persistentProgressService = persistentProgressService;
        }

        public void CreateFx(Vector2 at) => 
            _assetProvider.Instantiate(AssetsPath.Fx, at);

        public GameObject CreatePlayer(Vector2 at)
        {
            GameObject instantiate = _assetProvider.Instantiate(AssetsPath.Player, at);

            instantiate.GetComponent<MoveStateMachine>()?.Construct(_inputService, _staticDataService);
            instantiate.GetComponent<PlayerDie>()?.Construct(_reloadScene, _staticDataService);
            instantiate.GetComponent<PlayerAudio>()?.Construct(_staticDataService, _persistentProgressService);

            return instantiate;
        }

        public void CreateAudioPlayer(AudioConfigId id)
        {
            AudioPlayer audioPlayer = _assetProvider.Instantiate(AssetsPath.AudioPlayer).GetComponent<AudioPlayer>();
            audioPlayer?.Construct(_staticDataService, _persistentProgressService);
            audioPlayer.Play(id);
        }

        public GameObject CreatePlayerInLevelMap(MapLevelSlotContainer slotContainer, Vector2 at)
        {
            var instance = _assetProvider.Instantiate(AssetsPath.MapLevelPlayer, at);
            instance.GetComponent<PlayerMoveInMapLevel>()?.Construct(slotContainer);
            return instance;
        }

        public GameObject CreateFlySaw(Vector2 at, Vector3 scale)
        {
            GameObject instantiate = _assetProvider.Instantiate(AssetsPath.FlySaw, at);
            instantiate.transform.localScale = scale;
            return instantiate;
        }

        public GameObject CreateCmvCamera() =>
            _assetProvider.Instantiate(AssetsPath.CMVcam);
    }
}