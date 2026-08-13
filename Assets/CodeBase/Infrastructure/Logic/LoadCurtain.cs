using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure.Logic
{
    public class LoadCurtain
    {
        private const string CurtainPath = "Infrastructure/LoadinCurtain";

        private CanvasGroup _loadCurtain;

        private readonly CanvasGroup _curtainLink;
        private readonly ICoroutineRunner _coroutine;
        private bool _isCurtainEnable;

        public LoadCurtain(ICoroutineRunner coroutine)
        {
            _coroutine = coroutine;
            _curtainLink = Resources.Load<GameObject>(CurtainPath).GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            if (_isCurtainEnable)
                return;
            _isCurtainEnable = true;

            _loadCurtain = Object.Instantiate(_curtainLink);
        }

        public void Hide()
        {
            if (_isCurtainEnable == false)
                return;

            _coroutine.StartCoroutine(HideCurtain());
        }

        private IEnumerator HideCurtain()
        {
            do
            {
                _loadCurtain.alpha -= 0.04f;
                yield return null;
            } while (_loadCurtain.alpha > 0.0f);

            Object.Destroy(_loadCurtain.gameObject);
            _isCurtainEnable = false;
        }
    }
}