using CodeBase.Services;
using CodeBase.Services.RemoteConfig;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.Logic
{
    public class LoadCurtain
    {
        private const string CurtainKey = "LoadinCurtain";
        private const string InitCurtainKey = "InitCurtain";

        private CanvasGroup _loadCurtain;
        private float _windowAnimationSpeed;
        private AsyncOperationHandle<GameObject> _curtainHandle;
        private AsyncOperationHandle<GameObject> _initCurtainHandle;
        private CanvasGroup _curtainPrefab;
        private bool _isCurtainEnable;

        public async UniTask Init()
        {
            IRemoteConfigService remoteConfig = AllServices.Container.Single<IRemoteConfigService>();
            _windowAnimationSpeed = remoteConfig.GetWindowAnimationSpeed();

            _curtainHandle = Addressables.LoadAssetAsync<GameObject>(CurtainKey);
            GameObject prefab = await _curtainHandle.ToUniTask();
            _curtainPrefab = prefab.GetComponent<CanvasGroup>();
        }

        public async UniTask ShowInitCurtain()
        {
            _initCurtainHandle = Addressables.InstantiateAsync(InitCurtainKey);
            await _initCurtainHandle.ToUniTask();
        }
        public void HideInitCurtain()
        {
            if (_initCurtainHandle.IsValid())
                Addressables.Release(_initCurtainHandle);
        }

        public void Show()
        {
            if (_isCurtainEnable)
                return;
            _isCurtainEnable = true;

            _loadCurtain = UnityEngine.Object.Instantiate(_curtainPrefab);
        }

        public void Hide()
        {
            if (_isCurtainEnable == false)
                return;

            HideCurtain().Forget();
        }

        private async UniTask HideCurtain()
        {
            do
            {
                float delta = Mathf.Min(Time.deltaTime, 0.05f);
                _loadCurtain.alpha -= _windowAnimationSpeed * delta;
                await UniTask.Yield(PlayerLoopTiming.Update);

            } while (_loadCurtain.alpha > 0.0f);

            UnityEngine.Object.Destroy(_loadCurtain.gameObject);
            _isCurtainEnable = false;
        }
    }
}