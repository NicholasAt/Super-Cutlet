using CodeBase.Services.ReloadScene;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Audio;
using System;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerDie : MonoBehaviour
    {
        [SerializeField] private Collider2D _disableOnDie;
        [SerializeField] private PlayerAudio _playerAudio;
        public Action Happened;
        private IReloadSceneService _reloadScene;
        private IStaticDataService _staticDataService;

        public void Construct(IReloadSceneService reloadScene, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _reloadScene = reloadScene;
        }

        public void Die()
        {
            _playerAudio.Play(AudioConfigId.PlayerDie);
            _disableOnDie.enabled = false;
            Happened?.Invoke();
            Invoke(nameof(ReloadLevel), _staticDataService.PlayerData().RestartTimeAfterPlayerDeath);
        }

        private void ReloadLevel() =>
            _reloadScene.Reload();
    }
}