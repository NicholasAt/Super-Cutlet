using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Infrastructure.StatesMachine.States;
using CodeBase.MapLevel;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.MapLevelMenu
{
    public class TransferSelectLevelButton : BaseWindow
    {
        [SerializeField] private Button _transferButton;
        private MapLevelSlotContainer _slotContainer;
        private IGameStateMachine _gameStateMachine;

        public void Constructor(MapLevelSlotContainer slotContainer, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _slotContainer = slotContainer;
            _transferButton.onClick.AddListener(Transfer);
        }

        private void Transfer()
        {
            _transferButton.onClick.RemoveListener(Transfer);
            _gameStateMachine.Enter<LoadLevelState, string>(SelectedLevel());
        }

        private string SelectedLevel() =>
            $"Level {_slotContainer.ActiveIndex + 1}";
    }
}