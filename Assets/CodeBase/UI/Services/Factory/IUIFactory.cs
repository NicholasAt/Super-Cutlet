using CodeBase.Services;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows;
using System;
using System.Collections.Generic;
using CodeBase.Logic;
using CodeBase.MapLevel;
using CodeBase.UI.Windows.MapLevelMenu;

namespace CodeBase.UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        void CreateUIRoot();

        Dictionary<WindowId, BaseWindow> WindowsContainer { get; }
        Action<WindowId> OnWindowClose { get; set; }

        void Clean();

        TransferSelectLevelButton CreateLevelTransferButton(MapLevelSlotContainer slotContainer);

        void CreateUpdateTimer(TriggeredPlayer finishTrigger, LevelTimer levelTimer);
        void CreateInfoLevelText(MapLevelSlotContainer slotContainer);
        void CreateInput();
        void CreateMainMenu(IWindowService windowService);
        void CreateSettings(IWindowService windowService);
        void CreateLoadMainMenuStateButton();
    }
}