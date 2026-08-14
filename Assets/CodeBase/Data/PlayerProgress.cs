using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        [field: SerializeField] public LevelDataDictionary LevelDataDictionary { get; private set; }

        public PlayerProgress()
        {
            LevelDataDictionary = new LevelDataDictionary();
        }
    }
}