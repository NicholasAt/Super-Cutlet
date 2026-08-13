using CodeBase.Infrastructure.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Audio;
using CodeBase.StaticData.Player;
using System;
using System.Collections;
using CodeBase.Player.ClimbSideChecker;
using UnityEngine;

namespace CodeBase.Player.PlayerMove
{
    public class MoveClimb : IBaseMoveState
    {
        private readonly PlayerAudio _playerAudio;
        private readonly ClimbSideChecker.ClimbSideChecker _climbSideChecker;
        private readonly Rigidbody2D _rigidbody;
        private readonly GroundChecker _groundChecker;
        private readonly IInputService _inputService;
        private readonly MoveStateMachine _moveStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly PlayerClimbMoveConfig _config;

        public Action<bool> OnClimbJumpTimeElapsed;

        private float _currentJumpForceTime;
        private bool _isEntered;

        public MoveClimb(ClimbSideChecker.ClimbSideChecker climbSideChecker, Rigidbody2D rigidbody, GroundChecker groundChecker, IInputService inputService, MoveStateMachine moveStateMachine, ICoroutineRunner coroutineRunner, IStaticDataService dataService, PlayerAudio playerAudio)
        {
            _playerAudio = playerAudio;
            _climbSideChecker = climbSideChecker;
            _rigidbody = rigidbody;
            _groundChecker = groundChecker;
            _inputService = inputService;
            _moveStateMachine = moveStateMachine;
            _coroutineRunner = coroutineRunner;
            _config = dataService.PlayerData().ClimbMoveConfig;
        }

        public void Enter()
        {
            ClearVelocity(true);
            _inputService.OnJump += Jump;
            _isEntered = true;
            _coroutineRunner.StartCoroutine(JumpForceTimer());
        }

        public void Exit()
        {
            _inputService.OnJump -= Jump;
            _isEntered = false;
        }

        public void FixedUpdate()
        {
            if (ChangeStateCondition())
                _moveStateMachine.Enter<MoveGround>();
            else
                Move();
        }

        private void Move() =>
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, LimitedVelocityY());

        private bool ChangeStateCondition()
        {
            bool checkSide = _climbSideChecker.GetSide() != ClimbSideId.Right && _climbSideChecker.GetSide() != ClimbSideId.Left;
            return checkSide || _groundChecker.IsGround();
        }

        private void Jump()
        {
            _playerAudio.Play(AudioConfigId.Jump);
            ClearVelocity(false, true);
            _rigidbody.AddForce(Vector2.up * CalculateForceUp(), ForceMode2D.Impulse);
            _rigidbody.AddForce(Vector2.right * GetClimbSide(), ForceMode2D.Impulse);

            OnClimbJumpTimeElapsed?.Invoke(false);
            _coroutineRunner.StartCoroutine(ClimbJumpTimer());
        }

        private IEnumerator ClimbJumpTimer()
        {
            yield return new WaitForSeconds(_config.ClimbJumpTimerDelay);
            OnClimbJumpTimeElapsed?.Invoke(true);
        }

        private float LimitedVelocityY() =>
            (_rigidbody.velocity.y < _config.MaxVelocityDownSpeed) ? _config.MaxVelocityDownSpeed : _rigidbody.velocity.y;

        private IEnumerator JumpForceTimer()
        {
            while (_isEntered)
            {
                _currentJumpForceTime += Time.deltaTime;
                yield return null;
            }

            _currentJumpForceTime = 0;
        }

        private float CalculateForceUp()
        {
            float percentCurrentVelocity = Mathf.Clamp(_currentJumpForceTime, 0, _config.MaxTimeToMaxJumpForce) / _config.MaxTimeToMaxJumpForce;
            return _config.JumpForceUp * percentCurrentVelocity;
        }

        private float GetClimbSide()
        {
            ClimbSideId side = _climbSideChecker.GetSide();
            return (side == ClimbSideId.Right) ? -_config.JumpForceSide : (side == ClimbSideId.Left) ? _config.JumpForceSide : 0;
        }

        private void ClearVelocity(bool horizontal = false, bool vertical = false) =>
            _rigidbody.velocity = new Vector2(horizontal ? 0 : _rigidbody.velocity.x, vertical ? 0 : _rigidbody.velocity.y);
    }
}