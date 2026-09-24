using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine { get; }

        public Game()
        {
            StateMachine = new GameStateMachine(new SceneLoader(), new LoadCurtain(), AllServices.Container);
        }
    }
}