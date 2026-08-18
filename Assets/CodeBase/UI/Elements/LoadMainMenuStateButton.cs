using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Infrastructure.StatesMachine.States;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class LoadMainMenuStateButton : BaseWindow
    {
        [SerializeField] private Button _mainMenuButton;
        private IGameStateMachine _gameStateMachine;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _mainMenuButton.onClick.AddListener(MainMenuState);
        }

        private void MainMenuState() => 
            _gameStateMachine.Enter<LoadMainMenuState>();
    }
}