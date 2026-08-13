using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private const float Epsilon = 0.1f;
        private readonly int StateHash = Animator.StringToHash("State");
        private readonly int RootHash = Animator.StringToHash("Root");
        private readonly int DieHash = Animator.StringToHash("Die");

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PlayerDie _die;

        private enum MoveState
        {
            Idle = 0,
            Running = 1,
            Jump = 2,
            Fall = 3,
        }

        private MoveState _activeState = MoveState.Idle;
        private MoveState _oldState = MoveState.Idle;

        private void Awake()
        {
            _die.Happened += Pause;
        }

        private void Pause()
        {
            enabled = false;
            _animator.Play(DieHash);
        }

        private void Update()
        {
            Flip();
            UpdateState();
        }

        private void UpdateState()
        {
            UpdateMove();
            UpdateJump();
            SetState();
        }

        private void UpdateMove() =>
            _activeState = Mathf.Abs(_rigidbody.velocity.x) > Epsilon ? MoveState.Running : MoveState.Idle;

        private void UpdateJump()
        {
            _activeState = (_rigidbody.velocity.y > Epsilon) ? MoveState.Jump : (_rigidbody.velocity.y < -Epsilon) ? MoveState.Fall : _activeState;
        }

        private void SetState()
        {
            if (_oldState != _activeState)
            {
                _animator.Play(RootHash);
                _oldState = _activeState;
            }

            _animator.SetInteger(StateHash, (int)_activeState);
        }

        private void Flip()
        {
            _spriteRenderer.flipX = (_rigidbody.velocity.x < -Epsilon) || ((_rigidbody.velocity.x > Epsilon) ? false : _spriteRenderer.flipX);
            //_spriteRenderer.flipX = (_input.MoveAxis < 0) || ((_input.MoveAxis > 0) ? false : _spriteRenderer.flipX);
        }
    }
}