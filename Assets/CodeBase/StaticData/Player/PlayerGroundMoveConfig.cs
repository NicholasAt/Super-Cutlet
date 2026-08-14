using System;
using UnityEngine;

namespace CodeBase.StaticData.Player
{
    [Serializable]
    public class PlayerGroundMoveConfig
    {
        [field: SerializeField] public float SpeedMove { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float SmoothMoveSpeed { get; private set; }
    }
}