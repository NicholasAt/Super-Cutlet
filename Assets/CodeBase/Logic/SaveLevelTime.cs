using CodeBase.Data;
using CodeBase.Logic.Extension;
using CodeBase.Services.Analytic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Logic
{
    public class SaveLevelTime : MonoBehaviour
    {
        [SerializeField] private LevelTransfer _levelTransfer;
        [SerializeField] private LevelTimer _levelTimer;
        private ISaveLoadService _saveService;
        private IAnalytics _analytics;
        private IPersistentProgressService _persistentProgress;

        public void Construct(ISaveLoadService saveService, IPersistentProgressService persistentProgress, IAnalytics analytics)
        {
            _persistentProgress = persistentProgress;
            _saveService = saveService;
            _analytics = analytics;

            _levelTransfer.OnTransfer += Save;
        }

        private void Save()
        {
            _levelTransfer.OnTransfer -= Save;
            OverwriteOrInitNewLevelProgress();
            _saveService.SavePlayerProgress();
            _analytics.LevelCompleted(SceneManager.GetActiveScene().name, _levelTimer.GetSeconds().ToLevelTimeSeconds());
        }

        private void OverwriteOrInitNewLevelProgress()
        {
            if (_persistentProgress.Progress.LevelDataDictionary.Dictionary.TryGetValue(SceneKey(), out LevelData levelData))
            {
                if (levelData.TimeToCompleteLevel > _levelTimer.GetSeconds())
                    NewLevelData(_levelTimer.GetSeconds());
            }
            else
                NewLevelData(_levelTimer.GetSeconds());
        }

        private void NewLevelData(float timeToCompleteLevel) =>
            _persistentProgress.Progress.LevelDataDictionary.Dictionary[SceneKey()] = new LevelData(timeToCompleteLevel);

        private static string SceneKey() =>
            SceneManager.GetActiveScene().name;
    }
}