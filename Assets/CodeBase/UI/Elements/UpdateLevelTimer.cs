using CodeBase.Logic;
using CodeBase.Logic.Extension;
using CodeBase.UI.Windows;
using System.Collections;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class UpdateLevelTimer : BaseWindow
    {
        [SerializeField] private TMP_Text _timerText;
        private Coroutine _timerCoroutine;
        private TriggeredPlayer _finishPlayer;
        private LevelTimer _levelTimer;

        public void Construct(TriggeredPlayer finishPlayer, LevelTimer levelTimer)
        {
            _levelTimer = levelTimer;
            _finishPlayer = finishPlayer;
            finishPlayer.OnTriggeredEnter += StopUpdateTime;
        }

        private void Start()
        {
            _timerCoroutine = StartCoroutine(UpdateTime());
        }

        private IEnumerator UpdateTime()
        {
            var wait = new WaitForSeconds(0.1f);    //update speed
            while (true)
            {
                _timerText.text = _levelTimer.GetSeconds().ToLevelTime();
                yield return wait;
            }
        }

        private void StopUpdateTime()
        {
            _finishPlayer.OnTriggeredEnter -= StopUpdateTime;
            StopCoroutine(_timerCoroutine);
        }
    }
}