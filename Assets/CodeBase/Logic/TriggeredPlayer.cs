using System;
using UnityEngine;

namespace CodeBase.Logic
{
    public class TriggeredPlayer : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        public Collider2D Collider { get; private set; }

        public Action OnTriggeredEnter;
        public Action OnTriggeredExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(PlayerTag))
            {
                Collider = other;
                OnTriggeredEnter?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(PlayerTag))
            {
                Collider = other;
                OnTriggeredExit?.Invoke();
            }
        }
    }
}