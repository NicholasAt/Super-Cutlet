using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;

namespace CodeBase.Infrastructure.StatesMachine.States
{
    public class LoadProgressState : IState
    {
        private const string FirstLevelKey = "Level 1";

        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoad;
        private readonly IGameStateMachine _gameStateMachine;

        public LoadProgressState(IPersistentProgressService persistentProgressService, ISaveLoadService saveLoad, IGameStateMachine gameStateMachine)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoad = saveLoad;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            var progress = _saveLoad.LoadPlayerProgress();
            if (progress == null)
            {
                _persistentProgressService.NewPlayerProgress();
                _gameStateMachine.Enter<LoadLevelState, string>(FirstLevelKey);
            }
            else
            {
                _persistentProgressService.SetPlayerProgress(progress);
                //_gameStateMachine.Enter<LoadLevelState, string>(FirstLevelKey);
                _gameStateMachine.Enter<LoadMapLevelMenuState>();
            }
        }

        public void Exit()
        { }
    }
}