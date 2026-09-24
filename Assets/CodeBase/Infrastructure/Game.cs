using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine { get; }

        public Game(ICoroutineRunner runner)
        {
            StateMachine = new GameStateMachine(new SceneLoader(), new LoadCurtain(runner), AllServices.Container);
        }
    }
}