using UnityEngine;

namespace CodeBase.Logic
{
    public class DestroyOnCollision : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag(Constants.PlayerTag) == false)
                Destroy(gameObject);
        }
    }
}