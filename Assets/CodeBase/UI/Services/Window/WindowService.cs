using CodeBase.UI.Services.Factory;
using CodeBase.UI.Windows;
using Cysharp.Threading.Tasks;
using System;

namespace CodeBase.UI.Services.Window
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _uiFactory.OnWindowClose += RemoveInContainer;
        }

        public async UniTask Open(WindowId id)
        {
            switch (id)
            {
                case WindowId.None:
                case WindowId.TransferLevelButton:
                case WindowId.LevelTimer:
                case WindowId.MapLevelInfoLevelText:
                case WindowId.Input:
                    break;

                case WindowId.MainMenu:
                    await _uiFactory.CreateMainMenu(this);
                    break;

                case WindowId.Settings:
                    await _uiFactory.CreateSettings(this);
                    break;

                case WindowId.LoadMainMenuStateButton:
                    await _uiFactory.CreateLoadMainMenuStateButton();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(id), id, null);
            }
        }

        public void Close(WindowId id)
        {
            if (GetWindow(id, out BaseWindow window))
                window.Close();
        }

        public bool GetWindow<TWindow>(WindowId id, out TWindow window) where TWindow : BaseWindow
        {
            window = _uiFactory.WindowsContainer.TryGetValue(id, out BaseWindow valueWindow) ? (TWindow)valueWindow : null;
            return window;
        }

        private void RemoveInContainer(WindowId id) =>
            _uiFactory.WindowsContainer.Remove(id);
    }
}