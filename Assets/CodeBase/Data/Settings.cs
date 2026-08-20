using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class Settings
    {
        [field: SerializeField] public float AudioVolume { get; private set; }
        public Action OnChangeAudioVolume;

        public Settings(float audioVolume)
        {
            AudioVolume = audioVolume;
        }

        public void SetAudioVolume(float value)
        {
            AudioVolume = value;
            OnChangeAudioVolume?.Invoke();
        }

        public void UnSubscriber() =>
            OnChangeAudioVolume = null;
    }
}