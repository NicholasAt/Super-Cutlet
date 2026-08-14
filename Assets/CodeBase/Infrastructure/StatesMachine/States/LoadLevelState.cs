using Cinemachine;
using CodeBase.Infrastructure.Logic;
using CodeBase.Logic;
using CodeBase.Services.Factory;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData.Audio;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using UnityEngine;

namespace CodeBase.Infrastructure.StatesMachine.States
{
    public class LoadLevelState : IPayloadState<string>
    {
        private const string PlayerInitialPointTag = "PlayerInitialPoint";

        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IInputService _inputService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IWindowService _windowService;

        private SceneComponentContainer _componentContainer;

        public LoadLevelState(IGameStateMachine stateMachine, SceneLoader sceneLoader, LoadCurtain loadingCurtain, IGameFactory gameFactory, IUIFactory uiFactory, IInputService inputService, ISaveLoadService saveLoadService, IPersistentProgressService persistentProgressService, IWindowService windowService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _inputService = inputService;
            _saveLoadService = saveLoadService;
            _persistentProgressService = persistentProgressService;
            _windowService = windowService;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            Clean();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnLoaded()
        {
            _uiFactory.CreateUIRoot();
            FindComponentContainer();

            InitWorld();

            _stateMachine.Enter<LoopState>();
        }

        private void InitWorld()
        {
            _uiFactory.CreateInput();
            _windowService.Open(WindowId.LoadMainMenuStateButton);
            _gameFactory.CreateAudioPlayer(AudioConfigId.Levels);

            InitFinish();
            InitComponentsInScene();
            InitUpdateTimerText();
            GameObject player = _gameFactory.CreatePlayer(GameObject.FindGameObjectWithTag(PlayerInitialPointTag).transform.position);
            InitCamera(player.transform);
        }

        private void InitComponentsInScene()
        {
            InitSawSpawners();
            InitDestroyBlockTime();
        }

        private void InitFinish()
        {
            _componentContainer.Finish.GetComponent<SaveLevelTime>().Construct(_saveLoadService, _persistentProgressService);
            _componentContainer.Finish.GetComponent<LevelTransfer>().Construct(_stateMachine);
        }

        private void InitDestroyBlockTime() =>
            _componentContainer.BlockTimers.ForEach(x => x.Construct(_gameFactory));

        private void InitSawSpawners()
        {
            _componentContainer.SawSpawners.ForEach(x => x.Construct(_gameFactory));
            _componentContainer.SawSpawners.ForEach(x => x.StartSpawnTimer());
        }

        private void InitUpdateTimerText() =>
            _uiFactory.CreateUpdateTimer(_componentContainer.Finish.GetComponent<TriggeredPlayer>(), _componentContainer.Finish.GetComponent<LevelTimer>());

        private void FindComponentContainer() =>
            _componentContainer = Object.FindObjectOfType<SceneComponentContainer>();

        private void InitCamera(Transform playerTransform)
        {
            GameObject camera = _gameFactory.CreateCmvCamera();
            camera.GetComponent<CinemachineVirtualCamera>().Follow = playerTransform;
            camera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = _componentContainer.CameraConfinerCollider;
        }

        private void Clean()
        {
            _uiFactory.Clean();
            _inputService.Unsubscribe();
            _persistentProgressService.Settings.UnSubscriber();
        }
    }
}