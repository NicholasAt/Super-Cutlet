using CodeBase.Infrastructure.StatesMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        private async UniTaskVoid Awake()
        {
            Game game = new Game();
            await game.Run();
            game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}