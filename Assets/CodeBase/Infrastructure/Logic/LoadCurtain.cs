using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Logic
{
    public class LoadCurtain
    {
        private const string CurtainPath = "Infrastructure/LoadinCurtain";

        private CanvasGroup _loadCurtain;

        private readonly CanvasGroup _curtainLink;
        private bool _isCurtainEnable;

        public LoadCurtain()
        {
            _curtainLink = Resources.Load<GameObject>(CurtainPath).GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            if (_isCurtainEnable)
                return;
            _isCurtainEnable = true;

            _loadCurtain = UnityEngine.Object.Instantiate(_curtainLink);
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