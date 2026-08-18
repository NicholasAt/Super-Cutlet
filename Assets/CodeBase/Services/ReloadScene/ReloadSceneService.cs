using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Infrastructure.StatesMachine.States;
using UnityEngine.SceneManagement;

namespace CodeBase.Services.ReloadScene
{
    public class ReloadSceneService : IReloadSceneService
    {
        private const string Initial = "Initial";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadCurtain _loadCurtain;
        private string _reloadSceneName = string.Empty;

        public ReloadSceneService(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadCurtain loadCurtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadCurtain = loadCurtain;
        }

        public void Reload()
        {
            _loadCurtain.Show();
            _reloadSceneName = SceneManager.GetActiveScene().name;
            _sceneLoader.Load(Initial, EnterLoadLevelState);
        }

        private void EnterLoadLevelState() =>
            _stateMachine.Enter<LoadLevelState, string>(_reloadSceneName);
    }
}