namespace CodeBase.Player.PlayerMove
{
    public interface IBaseMoveState
    {
        void Enter();

        void Exit();

        void FixedUpdate();
    }
}