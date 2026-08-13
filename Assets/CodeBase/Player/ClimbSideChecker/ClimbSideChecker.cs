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

        public ClimbSideId GetSide()
        {
            int rightCount = Physics2D.OverlapBoxNonAlloc(transform.position + _cubePosition, _cubeSize, 0, _rightColliders, _climbLayerMask);
            int leftCount = Physics2D.OverlapBoxNonAlloc(transform.position + new Vector3(-_cubePosition.x, _cubePosition.y), _cubeSize, 0, _leftColliders, _climbLayerMask);

            return (rightCount > 0) ? ClimbSideId.Right : (leftCount > 0) ? ClimbSideId.Left : ClimbSideId.None;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position + _cubePosition, _cubeSize);
            Gizmos.DrawCube(transform.position + new Vector3(-_cubePosition.x, _cubePosition.y), _cubeSize);
        }
    }
}