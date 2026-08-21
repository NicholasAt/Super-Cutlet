using System;
using UnityEngine;

namespace CodeBase.StaticData.Audio
{
    [Serializable]
    public class AudioConfig
    {
        [SerializeField] private string _name = string.Empty;
        [field: SerializeField] public AudioConfigId ConfigId { get; private set; }
        [field: SerializeField] public AudioClip Clip { get; private set; }
    }
}