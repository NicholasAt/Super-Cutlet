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
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootKey = "UIRoot";

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

        public async UniTask CreateUIRoot()
        {
            GameObject prefab = await _assetProvider.LoadAsync<GameObject>(_staticDataService.UIRootReference());
            _uiRoot = Object.Instantiate(prefab).transform;
        }

        public async UniTask<TransferSelectLevelButton> CreateLevelTransferButton(MapLevelSlotContainer slotContainer)
        {
            TransferSelectLevelButton levelTransfer = await Instantiate<TransferSelectLevelButton>(WindowId.TransferLevelButton);
            levelTransfer.Constructor(slotContainer, _gameStateMachine);
            return levelTransfer;
        }

        public async UniTask CreateInfoLevelText(MapLevelSlotContainer slotContainer)
        {
            InfoLevelText infoLevelText = await Instantiate<InfoLevelText>(WindowId.MapLevelInfoLevelText);
            infoLevelText.Construct(_persistentProgressService, slotContainer);
        }

        public async UniTask CreateInput() =>
           await Instantiate<InputWindow>(WindowId.Input);

        public async UniTask CreateSettings(IWindowService windowService)
        {
            SettingsWindow window = await InstantiateRegister<SettingsWindow>(WindowId.Settings);
            window.GetComponentInChildren<OpenWindowButton>().Construct(windowService);
            window.GetComponent<ClearProgressButton>().Construct(_saveLoadService);

            foreach (AudioSlider slider in window.GetComponents<AudioSlider>())
            {
                slider.Construct(_saveLoadService, _persistentProgressService);
            }
        }

        public async UniTask CreateLoadMainMenuStateButton()
        {
            LoadMainMenuStateButton instantiate = await InstantiateRegister<LoadMainMenuStateButton>(WindowId.LoadMainMenuStateButton);
            instantiate.Construct(_gameStateMachine);
        }

        public async UniTask CreateMainMenu(IWindowService windowService)
        {
            MainMenu window = await InstantiateRegister<MainMenu>(WindowId.MainMenu);
            window.GetComponentInChildren<StartGameButton>()?.Construct(_gameStateMachine);
            window.GetComponentInChildren<OpenWindowButton>()?.Construct(windowService);
        }

        public async UniTask CreateUpdateTimer(TriggeredPlayer finishTrigger, LevelTimer levelTimer)
        {
            UpdateLevelTimer updateTimer = await Instantiate<UpdateLevelTimer>(WindowId.LevelTimer);
            updateTimer.Construct(finishTrigger, levelTimer);
        }

        private async UniTask<TWindow> InstantiateRegister<TWindow>(WindowId id) where TWindow : BaseWindow
        {
            TWindow window = await Instantiate<TWindow>(id);

            window.SetId(id);
            window.OnClosed += SendOnClosed;
            WindowsContainer[id] = window;
            return window;
        }

        private async UniTask<TWindow> Instantiate<TWindow>(WindowId id) where TWindow : BaseWindow
        {
            WindowConfig config = _staticDataService.ForWindow(id);
            GameObject prefab = await _assetProvider.LoadAsync<GameObject>(config.WindowReference);
            GameObject instance = Object.Instantiate(prefab, _uiRoot);

            if (instance.TryGetComponent(out TWindow window))
                return window;
            Debug.LogError("no component");
            return null;
        }

        private void SendOnClosed(WindowId id) =>
           OnWindowClose?.Invoke(id);
    }
}