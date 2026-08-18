using CodeBase.Data;
using CodeBase.Logic.Extension;
using CodeBase.MapLevel;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows.MapLevelMenu
{
    public class InfoLevelText : BaseWindow
    {
        [SerializeField] private TMP_Text _recordText;
        private IPersistentProgressService _persistentProgressService;
        private MapLevelSlotContainer _slotContainer;

        public void Construct(IPersistentProgressService persistentProgressService, MapLevelSlotContainer slotContainer)
        {
            _persistentProgressService = persistentProgressService;
            _slotContainer = slotContainer;
            slotContainer.OnClick += UpdateText;
        }

        private void Start() =>
            UpdateText();

        private void UpdateText() =>
            _recordText.text = GetInfoLevel(out var data) ? WriteRecord(data.TimeToCompleteLevel) : WriteNoRecord();

        private static string WriteRecord(float seconds) =>
            $"Record {seconds.ToLevelTime()}";

        private static string WriteNoRecord() =>
            "No Record";

        private bool GetInfoLevel(out LevelData data)
        {
            MapLevelSlot activeSlot = _slotContainer.Slots[_slotContainer.ActiveIndex];
            bool tryGetValue = _persistentProgressService.Progress.LevelDataDictionary.Dictionary.TryGetValue(activeSlot.LevelKey, out data);
            return tryGetValue;
        }
    }
}