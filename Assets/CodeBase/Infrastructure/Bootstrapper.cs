using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.StatesMachine.States;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            Game game = new Game();
            game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}