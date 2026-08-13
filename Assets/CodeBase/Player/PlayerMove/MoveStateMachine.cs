using CodeBase.Infrastructure.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.StaticData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Player.PlayerMove
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveStateMachine : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private ClimbSideChecker.ClimbSideChecker _climbSideChecker;
        [SerializeField] private PlayerAudio _playerAudio;
        private Rigidbody2D _rigidbody;
        private GroundChecker _groundChecker;

        private Dictionary<Type, IBaseMoveState> _states;
        private IBaseMoveState _activeState;
        private IStaticDataService _staticData;

        public void Construct(IInputService inputService, IStaticDataService staticData)
        {
            _staticData = staticData;
            _rigidbody = GetComponent<Rigidbody2D>();
            _groundChecker = new GroundChecker(_rigidbody);
            InitStates(inputService, _groundChecker, staticData);
        }

        private void Start()
        {
            SetRigidbodySettings();
            Enter<MoveGround>();
        }

        private void FixedUpdate() =>
            _activeState.FixedUpdate();

        public void Enter<TState>() where TState : class, IBaseMoveState
        {
            _activeState?.Exit();
            _activeState = _states[typeof(TState)];
            _activeState.Enter();
        }

        private void InitStates(IInputService inputService, GroundChecker groundChecker, IStaticDataService dataService)
        {
            var moveClimb = new MoveClimb(_climbSideChecker, _rigidbody, groundChecker, inputService, this, this, dataService, _playerAudio);
            var moveGround = new MoveGround(_climbSideChecker, _rigidbody, groundChecker, inputService, this, dataService, _playerAudio);
            moveClimb.OnClimbJumpTimeElapsed += moveGround.ClimbJumpTimeElapsed;

            _states = new Dictionary<Type, IBaseMoveState>
            {
                [typeof(MoveGround)] = moveGround,
                [typeof(MoveClimb)] = moveClimb,
            };
        }

        private void SetRigidbodySettings()
        {
            _rigidbody.gravityScale = _staticData.PlayerData().GravityScale;
        }
    }
}