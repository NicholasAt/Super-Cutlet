using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class Settings
    {
        [field: SerializeField] public float MusicVolume { get; private set; } = 1;
        [field: SerializeField] public float SFXVolume { get; private set; } = 1;

        public Action OnMusicChange { get; set; }
        public Action OnSFXChange { get; set; }

        public void SetSFXVolume(float value)
        {
            SFXVolume = value;
            OnSFXChange?.Invoke();
        }
        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
            OnMusicChange?.Invoke();
        }

        public void UnSubscriber()
        {
            OnSFXChange = null;
            OnMusicChange = null;
        }
    }
}