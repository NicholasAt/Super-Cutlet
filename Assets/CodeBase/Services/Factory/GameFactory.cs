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
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

        public async UniTask<GameObject> CreateFlySaw(Vector2 at, Vector3 scale, CancellationToken ct)
        {
            AssetReferenceGameObject reference = _staticDataService.GetAssetsData().FlySawReference;
            GameObject prefab = await _assetProvider.LoadAsync<GameObject>(reference, ct);
            GameObject instantiate = Object.Instantiate(prefab, at, Quaternion.identity);
            instantiate.transform.localScale = scale;
            return instantiate;
        }

        public async UniTask<GameObject> CreatePlayer(Vector2 at)
        {
            AssetReferenceGameObject reference = _staticDataService.PlayerData().PlayerReference;
            GameObject prefab = await _assetProvider.LoadAsync<GameObject>(reference);

            GameObject instantiate = Object.Instantiate(prefab, at, Quaternion.identity);

            instantiate.GetComponent<MoveStateMachine>()?.Construct(_inputService, _staticDataService);
            instantiate.GetComponent<PlayerDie>()?.Construct(_reloadScene, _staticDataService);
            instantiate.GetComponent<PlayerAudio>()?.Construct(_staticDataService, _persistentProgressService);

            return instantiate;
        }

        public async UniTask CreateFX(Vector2 at, CancellationToken ct)
        {
            GameObject prefab = await _assetProvider.LoadAsync<GameObject>(_staticDataService.GetAssetsData().FxReference, ct);
            Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public async UniTask CreateAudioPlayer(AudioConfigId id)
        {
            GameObject prefab = await _assetProvider.LoadAsync<GameObject>(_staticDataService.GetAssetsData().AudioPlayerReference);
            AudioPlayer audioPlayer = Object.Instantiate(prefab).GetComponent<AudioPlayer>();
            audioPlayer.Construct(_staticDataService, _persistentProgressService);
            audioPlayer.Play(id);
        }
        public async UniTask<GameObject> CreatePlayerInLevelMap(MapLevelSlotContainer slotContainer, Vector2 at)
        {
            GameObject prefab = await _assetProvider.LoadAsync<GameObject>(_staticDataService.GetAssetsData().MapLevelPlayerReference);
            GameObject instance = Object.Instantiate(prefab, at, Quaternion.identity);
            instance.GetComponent<PlayerMoveInMapLevel>()?.Construct(slotContainer);
            return instance;
        }
        public async UniTask<GameObject> CreateCmvCamera()
        {
            var prefab =await _assetProvider.LoadAsync<GameObject>(_staticDataService.GetAssetsData().CMVcamReference);
            return Object.Instantiate(prefab);
            //return _assetProvider.Instantiate(AssetsPath.CMVcam);
        }
    }
}