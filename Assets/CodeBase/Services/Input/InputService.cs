using System;

namespace CodeBase.Services.Input
{
    public class InputService : IInputService
    {
        public float MoveAxis => _mainControls.Move.Move.ReadValue<float>();
        public Action OnJump { get; set; }
        public bool Jump => _mainControls.Move.Jump.inProgress;

        private readonly MainControls _mainControls;
        public InputService()
        {
            _mainControls = new MainControls();
            _mainControls.Move.Enable();

            _mainControls.Move.Jump.performed += _ => OnJump?.Invoke();
        }

        public void Unsubscribe() =>
            OnJump = null;
    }
}