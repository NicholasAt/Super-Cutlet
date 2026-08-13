using UnityEngine;

namespace CodeBase.Logic.Traps
{
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform[] _wayPoints;
        private int _activeIndex;

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, GetWayPoint(), _speed * Time.deltaTime);
        }

        private Vector3 GetWayPoint()
        {
            var currentPoint = _wayPoints[_activeIndex].position;
            if (transform.position == currentPoint)
            {
                _activeIndex++;
                _activeIndex %= _wayPoints.Length;
            }

            return currentPoint;
        }
    }
}