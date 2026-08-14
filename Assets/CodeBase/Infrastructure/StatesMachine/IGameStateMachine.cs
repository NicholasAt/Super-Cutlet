using CodeBase.Services;

namespace CodeBase.Infrastructure.StatesMachine
{
    public interface IGameStateMachine : IService
    {
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;
        void Enter<TState>() where TState : class, IState;
    }
}