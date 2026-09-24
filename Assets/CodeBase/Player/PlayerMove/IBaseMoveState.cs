namespace CodeBase.Player.PlayerMove
{
    public interface IBaseMoveState
    {
        void Init();
        void Destroy();
        void Enter();

        void Exit();

        void FixedUpdate();
    }
}