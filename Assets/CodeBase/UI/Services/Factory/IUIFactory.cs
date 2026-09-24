using CodeBase.Logic;
using CodeBase.MapLevel;
using CodeBase.Services;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.MapLevelMenu;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace CodeBase.UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        Dictionary<WindowId, BaseWindow> WindowsContainer { get; }
        Action<WindowId> OnWindowClose { get; set; }

        void Clean();

        UniTask<TransferSelectLevelButton> CreateLevelTransferButton(MapLevelSlotContainer slotContainer);
        UniTask CreateUpdateTimer(TriggeredPlayer finishTrigger, LevelTimer levelTimer);
        UniTask CreateInfoLevelText(MapLevelSlotContainer slotContainer);
        UniTask CreateInput();
        UniTask CreateMainMenu(IWindowService windowService);
        UniTask CreateSettings(IWindowService windowService);
        UniTask CreateLoadMainMenuStateButton();
        UniTask CreateUIRoot();
    }
}