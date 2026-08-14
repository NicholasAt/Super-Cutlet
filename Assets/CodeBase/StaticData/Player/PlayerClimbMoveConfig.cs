using System;
using UnityEngine;

namespace CodeBase.StaticData.Player
{
    [Serializable]
    public class PlayerClimbMoveConfig
    {
        [field: SerializeField] public float ClimbJumpTimerDelay { get; private set; }
        [field: SerializeField] public float MaxVelocityDownSpeed { get; private set; }
        [field: SerializeField] public float JumpForceUp { get; private set; }
        [field: SerializeField] public float JumpForceSide { get; private set; }
        [field: SerializeField] public float MaxTimeToMaxJumpForce { get; private set; }
    }
}