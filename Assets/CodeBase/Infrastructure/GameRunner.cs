using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private Bootstrapper _bootstrapper;
        private void Awake()
        {
            Bootstrapper bootstrapper = FindAnyObjectByType<Bootstrapper>();
            if (bootstrapper == null)
            {
                Instantiate(_bootstrapper);
            }

            Destroy(gameObject);
        }
    }
}