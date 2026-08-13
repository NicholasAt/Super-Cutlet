using CodeBase.Player.ClimbSideChecker;
using CodeBase.Services.Input;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Audio;
using CodeBase.StaticData.Player;
using UnityEngine;

namespace CodeBase.Player.PlayerMove
{
    public class MoveGround : IBaseMoveState
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly ClimbSideChecker.ClimbSideChecker _climbSideChecker;
        private readonly GroundChecker _groundChecker;
        private readonly IInputService _input;
        private readonly MoveStateMachine _moveStateMachine;
        private readonly PlayerAudio _playerAudio;
        private readonly PlayerGroundMoveConfig _config;

        private bool _climbJumpTimerElapsed = true;

        public MoveGround(ClimbSideChecker.ClimbSideChecker climbSideChecker, Rigidbody2D rigidbody, GroundChecker groundChecker, IInputService input, MoveStateMachine moveStateMachine, IStaticDataService dataService, PlayerAudio playerAudio)
        {
            _climbSideChecker = climbSideChecker;
            _rigidbody = rigidbody;
            _groundChecker = groundChecker;
            _input = input;
            _moveStateMachine = moveStateMachine;
            _playerAudio = playerAudio;
            _config = dataService.PlayerData().GroundMoveConfig;
        }

        public void Enter()
        {
            _input.OnJump += Jump;
        }

        public void Exit()
        {
            _input.OnJump -= Jump;
        }

        public void FixedUpdate()
        {
            if (ChangeStateCondition())
                _moveStateMachine.Enter<MoveClimb>();
            else if (_climbJumpTimerElapsed)
                Move();
        }

        public void ClimbJumpTimeElapsed(bool isElapsed) =>
            _climbJumpTimerElapsed = isElapsed;

        private bool ChangeStateCondition() =>
            _groundChecker.IsGround() == false && _climbSideChecker.GetSide() != ClimbSideId.None;

        private void Move()
        {
            if (_groundChecker.IsGround() == false)
            {
                if (_input.MoveAxis == 0)
                    return;
            }
            _rigidbody.velocity = new Vector2(SmoothSpeed(), _rigidbody.velocity.y);
        }

        private float SmoothSpeed() =>
            Mathf.Lerp(_rigidbody.velocity.x, _config.SpeedMove * _input.MoveAxis, _config.SmoothMoveSpeed * Time.fixedDeltaTime);

        private void Jump()
        {
            if (_rigidbody == null)
                return;

            if (_groundChecker.IsGround() == false)
                return;

            _playerAudio.Play(AudioConfigId.Jump);
            _rigidbody.AddForce(Vector2.up * _config.JumpForce, ForceMode2D.Impulse);
        }
    }
}