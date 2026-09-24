using CodeBase.Data;
using CodeBase.Infrastructure.Logic;
using CodeBase.Services.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData.Audio;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StatesMachine.States
{
    public class LoadMainMenuState : IState
    {
        private const string MainScene = "MainMenu";

        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IWindowService _windowService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly LoadCurtain _loadCurtain;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameStateMachine _gameStateMachine;
        private bool _inProcess;
        public LoadMainMenuState(LoadCurtain loadCurtain, SceneLoader sceneLoader, IGameStateMachine gameStateMachine, IUIFactory uiFactory, IGameFactory gameFactory, IWindowService windowService, IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService)
        {
            _uiFactory = uiFactory;
            _gameFactory = gameFactory;
            _windowService = windowService;
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
            _loadCurtain = loadCurtain;
            _sceneLoader = sceneLoader;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            if (_inProcess)
                return;
            _inProcess = true;

            _loadCurtain.Show();
            Clean();
            _sceneLoader.LoadSingle(MainScene, OnLoaded).Forget();
        }

        public void Exit()
        {
            _inProcess = false;
            _loadCurtain.Hide();
        }

        private void OnLoaded()
        {
            LoadSettingsOrInitNew();

            _uiFactory.CreateUIRoot();
            _windowService.Open(WindowId.MainMenu);
            _gameFactory.CreateAudioPlayer(AudioConfigId.MainMenu);
            _gameStateMachine.Enter<LoopState>();
        }

        private void LoadSettingsOrInitNew()
        {
            Settings settings = _saveLoadService.LoadSettings();

            if (settings == null)
                _persistentProgressService.NewSettings(1f);
            else
                _persistentProgressService.SetSettings(settings);
        }

        private void Clean()
        {
            _persistentProgressService.Settings?.UnSubscriber();
        }
    }
}