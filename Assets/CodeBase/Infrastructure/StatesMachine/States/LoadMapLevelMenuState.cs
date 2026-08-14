using CodeBase.Infrastructure.Logic;
using CodeBase.MapLevel;
using CodeBase.Services.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Audio;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using UnityEngine;

namespace CodeBase.Infrastructure.StatesMachine.States
{
    public class LoadMapLevelMenuState : IState
    {
        private const string MapLevelKey = "MapLevel";
        private const string PlayerInitialPointTag = "PlayerInitial";

        private readonly IGameStateMachine _gameStateMachine;
        private readonly LoadCurtain _loadCurtain;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IWindowService _windowService;

        private MapLevelComponentContainer _componentContainer;

        public LoadMapLevelMenuState(SceneLoader sceneLoader, LoadCurtain loadCurtain, IGameStateMachine gameStateMachine, IGameFactory gameFactory, IUIFactory uiFactory, IPersistentProgressService persistentProgressService, IWindowService windowService)
        {
            _gameStateMachine = gameStateMachine;
            _loadCurtain = loadCurtain;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _persistentProgressService = persistentProgressService;
            _windowService = windowService;
        }

        public void Enter()
        {
            _loadCurtain.Show();
            Clean();
            _sceneLoader.Load(MapLevelKey, OnLoaded);
        }

        public void Exit()
        {
            _loadCurtain.Hide();
        }

        private void OnLoaded()
        {
            _uiFactory.CreateUIRoot();
            FindComponentContainer();
            _gameFactory.CreateAudioPlayer(AudioConfigId.MapLevel);
            InitSlotContainer();
            InitInfoLevelText();
            InitTransferLevelButton();
            InitPlayer();
            _windowService.Open(WindowId.LoadMainMenuStateButton);

            _gameStateMachine.Enter<LoopState>();
        }

        private void InitSlotContainer()
        {
            _componentContainer.SlotContainer.Construct(_persistentProgressService);
            _componentContainer.SlotContainer.InitSlots();
        }

        private void InitInfoLevelText() =>
            _uiFactory.CreateInfoLevelText(_componentContainer.SlotContainer);

        private void InitTransferLevelButton() =>
            _uiFactory.CreateLevelTransferButton(_componentContainer.SlotContainer);

        private void InitPlayer() =>
            _gameFactory.CreatePlayerInLevelMap(_componentContainer.SlotContainer, GameObject.FindGameObjectWithTag(PlayerInitialPointTag).transform.position);

        private void FindComponentContainer() =>
            _componentContainer = Object.FindObjectOfType<MapLevelComponentContainer>();

        private void Clean()
        {
            _persistentProgressService.Settings.UnSubscriber();
        }
    }
}