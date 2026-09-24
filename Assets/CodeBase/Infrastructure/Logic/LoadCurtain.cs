using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.Logic
{
    public class LoadCurtain
    {
        private const string CurtainKey = "LoadinCurtain";

        private CanvasGroup _loadCurtain;
        private AsyncOperationHandle<GameObject> _curtainHandle;
        private CanvasGroup _curtainPrefab;
        private bool _isCurtainEnable;

        public async UniTask Init()
        {
            _curtainHandle = Addressables.LoadAssetAsync<GameObject>(CurtainKey);
            GameObject prefab = await _curtainHandle.ToUniTask();
            _curtainPrefab = prefab.GetComponent<CanvasGroup>();
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
                _loadCurtain.alpha -= Constants.WindowAnimationSpeed * delta;
                await UniTask.Yield(PlayerLoopTiming.Update);

            } while (_loadCurtain.alpha > 0.0f);

            UnityEngine.Object.Destroy(_loadCurtain.gameObject);
            _isCurtainEnable = false;
        }
    }
}