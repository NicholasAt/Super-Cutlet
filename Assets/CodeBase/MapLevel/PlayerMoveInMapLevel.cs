using UnityEngine;

namespace CodeBase.MapLevel
{
    public class PlayerMoveInMapLevel : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _offsetY;
        private MapLevelSlotContainer _slotContainer;
        private int _currentPointIndex;

        public void Construct(MapLevelSlotContainer slotContainer)
        {
            _slotContainer = slotContainer;
        }

        private void Update()
        {
            Vector2 pointToMove = PointToMove();
            Move(pointToMove);

            if (ConditionToChangeMovePoint(pointToMove))
                CalculateMoveToPoint();
        }

        private void Move(Vector2 pointPosition) =>
            transform.position = Vector2.Lerp(transform.position, pointPosition, _speed * Time.deltaTime);

        private bool ConditionToChangeMovePoint(Vector2 pointPosition) =>
            Vector2.Distance(transform.position, pointPosition) < 0.5f; //allowable range

        private void CalculateMoveToPoint()
        {
            if (_slotContainer.ActiveIndex > _currentPointIndex)
                _currentPointIndex++;
            else if (_slotContainer.ActiveIndex < _currentPointIndex)
                _currentPointIndex--;
        }

        private Vector2 PointToMove() =>
            new Vector2(_slotContainer.Slots[_currentPointIndex].transform.position.x, _slotContainer.Slots[_currentPointIndex].transform.position.y + _offsetY);
    }
}