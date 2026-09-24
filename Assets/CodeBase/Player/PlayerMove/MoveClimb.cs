using CodeBase.Player.ClimbSideChecker;
using CodeBase.Services.Input;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Audio;
using CodeBase.StaticData.Player;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
        private readonly PlayerClimbMoveConfig _config;
        private CancellationTokenSource _cts;
        public Action<bool> OnClimbJumpTimeElapsed;

        private float _currentJumpForceTime;
        private bool _isEntered;

        public MoveClimb(ClimbSideChecker.ClimbSideChecker climbSideChecker, Rigidbody2D rigidbody, GroundChecker groundChecker, IInputService inputService, MoveStateMachine moveStateMachine, IStaticDataService dataService, PlayerAudio playerAudio)
        {
            _playerAudio = playerAudio;
            _climbSideChecker = climbSideChecker;
            _rigidbody = rigidbody;
            _groundChecker = groundChecker;
            _inputService = inputService;
            _moveStateMachine = moveStateMachine;
            _config = dataService.PlayerData().ClimbMoveConfig;
        }

        public void Init()
        {
            _cts = new CancellationTokenSource();
        }

        public void Destroy()
        {
            _cts.Cancel();
            _cts.Dispose();
            Exit();
        }

        public void Enter()
        {
            ClearVelocity(true);
            _inputService.OnJump += Jump;
            _isEntered = true;
            JumpForceTimer(_cts.Token).Forget();
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
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, LimitedVelocityY());

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
            ClimbJumpTimer(_cts.Token).Forget();
        }

        private async UniTask ClimbJumpTimer(CancellationToken ct)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_config.ClimbJumpTimerDelay), cancellationToken: ct);
                OnClimbJumpTimeElapsed?.Invoke(true);
            }
            catch (OperationCanceledException)
            {
                //ignore
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }
        private async UniTaskVoid JumpForceTimer(CancellationToken ct)
        {
            try
            {
                while (_isEntered)
                {
                    ct.ThrowIfCancellationRequested();
                    _currentJumpForceTime += Time.deltaTime;
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                }
                _currentJumpForceTime = 0;

            }
            catch (OperationCanceledException)
            {
                //ignore
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        private float LimitedVelocityY() =>
            (_rigidbody.linearVelocity.y < _config.MaxVelocityDownSpeed) ? _config.MaxVelocityDownSpeed : _rigidbody.linearVelocity.y;

        private float CalculateForceUp()
        {
            float percentCurrentVelocity = Mathf.Clamp(_currentJumpForceTime, _config.MinTimeToMaxJumpForce, _config.MaxTimeToMaxJumpForce) / _config.MaxTimeToMaxJumpForce;
            return _config.JumpForceUp * percentCurrentVelocity;
        }

        private float GetClimbSide()
        {
            ClimbSideId side = _climbSideChecker.GetSide();
            return (side == ClimbSideId.Right) ? -_config.JumpForceSide : (side == ClimbSideId.Left) ? _config.JumpForceSide : 0;
        }

        private void ClearVelocity(bool horizontal = false, bool vertical = false) =>
            _rigidbody.linearVelocity = new Vector2(horizontal ? 0 : _rigidbody.linearVelocity.x, vertical ? 0 : _rigidbody.linearVelocity.y);
    }
}