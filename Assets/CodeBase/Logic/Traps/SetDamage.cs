using CodeBase.Player;
using UnityEngine;

namespace CodeBase.Logic.Traps
{
    public class SetDamage : MonoBehaviour
    {
        [SerializeField] private TriggeredPlayer _triggered;
        private void Awake() => _triggered.OnTriggeredEnter += Kill;
        private void Kill() => _triggered.Collider.GetComponent<PlayerDie>().Die();
    }
}