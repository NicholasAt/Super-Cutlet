using UnityEngine;

namespace CodeBase.Infrastructure.Logic
{
    public class FlySawMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private Vector2 _direction = Vector2.zero;

        private void Update() => 
            transform.Translate(_direction * _speed * Time.deltaTime);

        public void StartMove(Vector2 direction) => 
            _direction = direction;
    }
}