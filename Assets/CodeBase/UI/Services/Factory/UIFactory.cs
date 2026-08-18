using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.StatesMachine;
using CodeBase.Logic;
using CodeBase.MapLevel;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.MainMenu;
using CodeBase.UI.Windows.MapLevelMenu;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";

        public Dictionary<WindowId, BaseWindow> WindowsContainer { get; } = new Dictionary<WindowId, BaseWindow>();
        public Action<WindowId> OnWindowClose { get; set; }

        private Transform _uiRoot;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;

        public UIFactory(IAssetProvider assetProvider, IStaticDataService staticDataService, IGameStateMachine gameStateMachine, IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
        }

        public void Clean()
        {
            WindowsContainer.Clear();
            OnWindowClose = null;
        }

        public void CreateUIRoot() =>
            _uiRoot = _assetProvider.Instantiate(UIRootPath).transform;

        public TransferSelectLevelButton CreateLevelTransferButton(MapLevelSlotContainer slotContainer)
        {
            TransferSelectLevelButton levelTransfer = Instantiate<TransferSelectLevelButton>(WindowId.TransferLevelButton);
            levelTransfer.Constructor(slotContainer, _gameStateMachine);
            return levelTransfer;
        }

        public void CreateInfoLevelText(MapLevelSlotContainer slotContainer)
        {
            InfoLevelText infoLevelText = Instantiate<InfoLevelText>(WindowId.MapLevelInfoLevelText);
            infoLevelText.Construct(_persistentProgressService, slotContainer);
        }

        public void CreateInput() =>
            Instantiate<InputWindow>(WindowId.Input);

        public void CreateSettings(IWindowService windowService)
        {
            SettingsWindow window = InstantiateRegister<SettingsWindow>(WindowId.Settings);
            window.GetComponent<AudioSlider>()?.Construct(_saveLoadService, _persistentProgressService);
            window.GetComponentInChildren<OpenWindowButton>()?.Construct(windowService);
            window.GetComponent<ClearProgressButton>()?.Construct(_saveLoadService);
        }

        public void CreateLoadMainMenuStateButton()
        {
            LoadMainMenuStateButton instantiate = InstantiateRegister<LoadMainMenuStateButton>(WindowId.LoadMainMenuStateButton);
            instantiate.Construct(_gameStateMachine);
        }

        public void CreateMainMenu(IWindowService windowService)
        {
            MainMenu window = InstantiateRegister<MainMenu>(WindowId.MainMenu);
            window.GetComponentInChildren<StartGameButton>()?.Construct(_gameStateMachine);
            window.GetComponentInChildren<OpenWindowButton>()?.Construct(windowService);
        }

        public void CreateUpdateTimer(TriggeredPlayer finishTrigger, LevelTimer levelTimer)
        {
            UpdateLevelTimer updateTimer = Instantiate<UpdateLevelTimer>(WindowId.LevelTimer);
            updateTimer.Construct(finishTrigger, levelTimer);
        }

        private TWindow InstantiateRegister<TWindow>(WindowId id) where TWindow : BaseWindow
        {
            TWindow window = Instantiate<TWindow>(id);

            window.SetId(id);
            window.OnClosed += SendOnClosed;
            WindowsContainer[id] = window;
            return window;
        }

        private TWindow Instantiate<TWindow>(WindowId id) where TWindow : BaseWindow
        {
            WindowConfig config = _staticDataService.ForWindow(id);
            return (TWindow)Object.Instantiate(config.Template, _uiRoot);
        }

        private void SendOnClosed(WindowId id) =>
           OnWindowClose?.Invoke(id);
    }
}