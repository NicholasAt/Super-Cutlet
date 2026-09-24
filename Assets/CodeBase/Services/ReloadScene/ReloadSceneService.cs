using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Infrastructure.StatesMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CodeBase.Services.ReloadScene
{
    public class ReloadSceneService : IReloadSceneService
    {
        private const string ReloadKey = "Reload";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadCurtain _loadCurtain;
        private string _reloadSceneName = string.Empty;
        private bool _inProcess;
        public ReloadSceneService(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadCurtain loadCurtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadCurtain = loadCurtain;
        }

        public void Reload()
        {
            if (_inProcess)
                return;
            _inProcess = true;

            _loadCurtain.Show();
            _reloadSceneName = SceneManager.GetActiveScene().name;
            _sceneLoader.LoadSingle(ReloadKey, EnterLoadLevelState).Forget();
        }

        private void EnterLoadLevelState()
        {
            _inProcess = false;
            _stateMachine.Enter<LoadLevelState, string>(_reloadSceneName);
        }
    }
}