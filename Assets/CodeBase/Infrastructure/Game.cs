using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Services;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine { get; }
        private readonly LoadCurtain _curtain;
        public Game()
        {
            StateMachine = new GameStateMachine(new SceneLoader(), _curtain = new LoadCurtain(), AllServices.Container);
        }
        public async UniTask Run()
        {
            await _curtain.Init();
        }
    }
}