using UnityEngine;

namespace CodeBase.Player.ClimbSideChecker
{
    public class ClimbSideChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _climbLayerMask;
        [SerializeField] private Vector2 _cubeSize;
        [SerializeField] private Vector3 _cubePosition;

        private readonly Collider2D[] _rightColliders = new Collider2D[1];
        private readonly Collider2D[] _leftColliders = new Collider2D[1];

        private ContactFilter2D _filter;

        private void Awake()
        {
            _filter = new();
            _filter.SetLayerMask(_climbLayerMask);
            _filter.useTriggers = false;
        }
        public ClimbSideId GetSide()
        {
            int rightCount = Physics2D.OverlapBox(transform.position + _cubePosition, _cubeSize, 0f, _filter, _rightColliders);
            int leftCount = Physics2D.OverlapBox(transform.position + new Vector3(-_cubePosition.x, _cubePosition.y), _cubeSize, 0f, _filter, _leftColliders);
            return (rightCount > 0) ? ClimbSideId.Right : (leftCount > 0) ? ClimbSideId.Left : ClimbSideId.None;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + _cubePosition, _cubeSize);
            Gizmos.DrawCube(transform.position + new Vector3(-_cubePosition.x, _cubePosition.y), _cubeSize);
        }
    }
}