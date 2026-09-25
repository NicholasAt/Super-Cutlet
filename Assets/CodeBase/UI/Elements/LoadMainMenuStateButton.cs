using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Infrastructure.StatesMachine.States;
using CodeBase.Logic.Extension;
using CodeBase.Services.Analytic;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class LoadMainMenuStateButton : BaseWindow
    {
        [SerializeField] private Button _mainMenuButton;

        private IGameStateMachine _gameStateMachine;
        private IAnalytics _analytics;

        public void Construct(IGameStateMachine gameStateMachine, IAnalytics analytics)
        {
            _gameStateMachine = gameStateMachine;
            _analytics = analytics;
            _mainMenuButton.onClick.AddListener(MainMenuState);
        }
        
        private void MainMenuState()
        {
            if (IsSendLeave())
                _analytics.Leave(SceneManager.GetActiveScene().name, Time.timeSinceLevelLoad.ToLevelTimeSeconds());
            _gameStateMachine.Enter<LoadMainMenuState>();
        }
        private bool IsSendLeave()
        {
            return SceneManager.GetActiveScene().name.StartsWith("Level");
        }
    }
}