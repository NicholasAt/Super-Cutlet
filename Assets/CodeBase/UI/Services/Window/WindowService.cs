using CodeBase.UI.Services.Factory;
using CodeBase.UI.Windows;
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

        public void Open(WindowId id)
        {
            switch (id)
            {
                case WindowId.None:
                    break;

                case WindowId.TransferLevelButton:
                    break;

                case WindowId.LevelTimer:
                    break;

                case WindowId.MapLevelInfoLevelText:
                    break;

                case WindowId.Input:
                    break;

                case WindowId.MainMenu:
                    _uiFactory.CreateMainMenu(this);
                    break;

                case WindowId.Settings:
                    _uiFactory.CreateSettings(this);
                    break;

                case WindowId.LoadMainMenuStateButton:
                    _uiFactory.CreateLoadMainMenuStateButton();
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
            window = _uiFactory.WindowsContainer.TryGetValue(id, out var valueWindow) ? (TWindow)valueWindow : null;
            return window;
        }

        private void RemoveInContainer(WindowId id) =>
            _uiFactory.WindowsContainer.Remove(id);
    }
}