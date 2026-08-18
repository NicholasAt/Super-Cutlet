using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Infrastructure.StatesMachine.States;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.MapLevelMenu
{
    public class StartGameButton : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        private IGameStateMachine _gameStateMachine;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _startGameButton.onClick.AddListener(StartGame);
        }

        private void StartGame() =>
            _gameStateMachine.Enter<LoadProgressState>();
    }
}