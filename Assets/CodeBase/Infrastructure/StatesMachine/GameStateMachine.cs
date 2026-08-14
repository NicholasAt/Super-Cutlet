using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.StatesMachine.States;
using CodeBase.Services;
using CodeBase.Services.Factory;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;

namespace CodeBase.Infrastructure.StatesMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitable> _states;
        private IExitable _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadCurtain loadCurtain, AllServices services)
        {
            _states = new Dictionary<Type, IExitable>
            {
                [typeof(LoopState)] = new LoopState(),
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, loadCurtain, services),

                [typeof(LoadMainMenuState)] = new LoadMainMenuState(loadCurtain, sceneLoader,
                    services.Single<IGameStateMachine>(),
                    services.Single<IUIFactory>(),
                    services.Single<IGameFactory>(),
                    services.Single<IWindowService>(),
                    services.Single<IPersistentProgressService>(),
                    services.Single<ISaveLoadService>()),

                [typeof(LoadProgressState)] = new LoadProgressState(
                    services.Single<IPersistentProgressService>(),
                    services.Single<ISaveLoadService>(),
                    services.Single<IGameStateMachine>()),

                [typeof(LoadMapLevelMenuState)] = new LoadMapLevelMenuState(sceneLoader, loadCurtain,
                    services.Single<IGameStateMachine>(),
                    services.Single<IGameFactory>(),
                    services.Single<IUIFactory>(),
                    services.Single<IPersistentProgressService>(),
                    services.Single<IWindowService>()),

                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadCurtain,
                    services.Single<IGameFactory>(),
                    services.Single<IUIFactory>(),
                    services.Single<IInputService>(),
                    services.Single<ISaveLoadService>(),
                    services.Single<IPersistentProgressService>(),
                    services.Single<IWindowService>()),
            };
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitable
        {
            _activeState?.Exit();
            TState state = _states[typeof(TState)] as TState;
            _activeState = state;
            return state;
        }
    }
}