using CodeBase.Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.MainMenu
{
    public class ClearProgressButton : MonoBehaviour
    {
        [SerializeField] private Button _clearProgressButton;
        private ISaveLoadService _saveLoadService;

        public void Construct(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _clearProgressButton.onClick.AddListener(ClearProgress);
        }

        private void ClearProgress() => 
            _saveLoadService.ClearPlayerProgress();
    }
}