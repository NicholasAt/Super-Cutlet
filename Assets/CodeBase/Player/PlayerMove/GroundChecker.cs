using UnityEngine;

namespace CodeBase.Player.PlayerMove
{
    public class GroundChecker
    {
        private readonly Rigidbody2D _rigidbody;
        public GroundChecker(Rigidbody2D rigidbody) => 
            _rigidbody = rigidbody;
        public bool IsGround() => 
            Mathf.Abs(_rigidbody.velocity.y) < 0.1f; //Epsilon
    }
}