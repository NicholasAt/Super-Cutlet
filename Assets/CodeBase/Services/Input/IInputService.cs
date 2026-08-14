using System;

namespace CodeBase.Services.Input
{
    public interface IInputService : IService
    {
        Action OnJump { get; set; }
        float MoveAxis { get; }
        bool Jump { get; }
        void Unsubscribe();
    }
}