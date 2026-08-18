using CodeBase.Services.PersistentProgress;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.MapLevel
{
    public class MapLevelSlotContainer : MonoBehaviour
    {
        [SerializeField] private Sprite _unlockSprite;
        public List<MapLevelSlot> Slots { get; private set; } = new List<MapLevelSlot>();
        public Action OnClick;
        public int ActiveIndex { get; private set; }
        private IPersistentProgressService _persistentProgressService;

        public void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;
        }

        public void InitSlots()
        {
            int levelCompletedCount = _persistentProgressService.Progress.LevelDataDictionary.Dictionary.Count + 1;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out MapLevelSlot levelSlot))
                {
                    if (levelCompletedCount > 0)
                    {
                        levelCompletedCount--;
                        levelSlot.UnLock(_unlockSprite);
                    }

                    string levelKey = $"Level {i + 1}";
                    levelSlot.Initialize(i, levelKey);
                    levelSlot.OnClick += SelectSlot;
                    Slots.Add(levelSlot);
                }
            }
        }

        private void SelectSlot(int index)
        {
            ActiveIndex = index;
            OnClick?.Invoke();
        }
    }
}