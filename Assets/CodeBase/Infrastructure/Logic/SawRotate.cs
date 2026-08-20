using UnityEngine;

namespace CodeBase.Infrastructure.Logic
{
    public class SawRotate : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform _pivot;
        private float _currentZ;

        private void Update()
        {
            _currentZ += _speed * 10f * Time.deltaTime;
            _currentZ %= 360f;
            _pivot.rotation = Quaternion.Euler(0, 0, _currentZ);
        }
    }
}