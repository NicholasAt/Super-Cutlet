using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class LevelData
    {
        [field: SerializeField] public float TimeToCompleteLevel { get; private set; }

        public LevelData(float timeToCompleteLevel)
        {
            TimeToCompleteLevel = timeToCompleteLevel;
        }
    }
}