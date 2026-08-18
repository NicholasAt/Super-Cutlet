using CodeBase.UI.Services.Window;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private WindowId _openId;
        [SerializeField] private WindowId _closeId;
        private IWindowService _windowService;

        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
            _openButton.onClick.AddListener(OpenAndClose);
        }

        private void OpenAndClose()
        {
            _windowService.Open(_openId);
            _windowService.Close(_closeId);
        }
    }
}