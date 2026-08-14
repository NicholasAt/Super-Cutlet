using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerInventory
    {
        [field: SerializeField] public int Cherry { get; private set; }

        public Action OnCollected;

        public void Collect()
        {
            Cherry++;
            OnCollected?.Invoke();
        }

        public void Unsubscribe() =>
            OnCollected = null;
    }
}