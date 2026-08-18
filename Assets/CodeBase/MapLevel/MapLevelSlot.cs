using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.MapLevel
{
    public class MapLevelSlot : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Action<int> OnClick;
        public int Index { get; private set; }
        public string LevelKey { get; private set; } = string.Empty;
        private bool _isLock = true;

        public void Initialize(int slotIndex, string levelKey)
        {
            Index = slotIndex;
            LevelKey = levelKey;
        }

        public void UnLock(Sprite unLockSprite)
        {
            _spriteRenderer.sprite = unLockSprite;
            _isLock = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isLock)
                return;

            OnClick?.Invoke(Index);
        }
    }
}