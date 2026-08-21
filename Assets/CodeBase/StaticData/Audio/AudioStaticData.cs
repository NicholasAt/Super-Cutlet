using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Audio
{
    [CreateAssetMenu(fileName = "New Audio Data", menuName = "Static Data/AudioData")]
    public class AudioStaticData : ScriptableObject
    {
        [field: SerializeField] public List<AudioConfig> Configs { get; private set; }

        [Range(0, 1f)] [SerializeField] private float _volume;
        public Action OnAudioVolumeChanged;

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                _volume = Mathf.Clamp01(_volume);
                OnAudioVolumeChanged?.Invoke();
            }
        }
    }
}