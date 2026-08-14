using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Logic;
using CodeBase.Services;
using CodeBase.Services.Factory;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.ReloadScene;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;

namespace CodeBase.Infrastructure.StatesMachine.States
{
    public class BootstrapState : IState
    {
        private const string InitScene = "Initial";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadCurtain _loadCurtain;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadCurtain loadCurtain, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadCurtain = loadCurtain;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(InitScene, EnterLoadMenu);
        }

        public void Exit()
        { }

        private void RegisterServices()
        {
            RegisterStaticData();
            _services.RegisterSingle<IInputService>(new InputService());
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>()));
            _services.RegisterSingle<IReloadSceneService>(new ReloadSceneService(_stateMachine, _sceneLoader, _loadCurtain));
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(), _services.Single<IInputService>(), _services.Single<IStaticDataService>(), _services.Single<IReloadSceneService>(), _services.Single<IPersistentProgressService>()));
            _services.RegisterSingle<IUIFactory>(new UIFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>(), _services.Single<IGameStateMachine>(), _services.Single<IPersistentProgressService>(), _services.Single<ISaveLoadService>()));
            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));
        }

        private void EnterLoadMenu()
        {
            _stateMachine.Enter<LoadMainMenuState>();
        }

        private void RegisterStaticData()
        {
            var service = new StaticDataService();
            service.Load();
            _services.RegisterSingle<IStaticDataService>(service);
        }
    }
}