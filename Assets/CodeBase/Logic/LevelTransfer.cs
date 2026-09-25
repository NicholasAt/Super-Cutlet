using System;
using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Infrastructure.StatesMachine.States;
using CodeBase.Services.Analytic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Logic
{
    public class LevelTransfer : MonoBehaviour
    {
        [SerializeField] private string _transferTo = string.Empty;
        [SerializeField] private TriggeredPlayer _triggered;
        public Action OnTransfer;

        private IGameStateMachine _stateMachine;
        private IAnalytics _analytics;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
            _triggered.OnTriggeredEnter += Transfer;
        }

        private void Transfer()
        {
            _triggered.OnTriggeredEnter -= Transfer;
            OnTransfer?.Invoke();
            _stateMachine.Enter<LoadLevelState, string>(_transferTo);
        }
    }
}