using Cysharp.Threading.Tasks;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.UnityConsent;

namespace CodeBase.Services.RemoteConfig
{
    public class RemoteConfig : IRemoteConfigService
    {
        private float _uIAnimationSpeed;

        public struct userAttributes { }
        public struct appAttributes { }
        private bool _running;

        public async UniTask Check()
        {
            _running = true;
            try
            {
                await UnityServices.InitializeAsync().AsUniTask();

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    try
                    {
                        await AuthenticationService.Instance.SignInAnonymouslyAsync().AsUniTask();
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"Auth failed (offline?): {e.Message}");
                    }
                }
                EndUserConsent.SetConsentState(new ConsentState { AnalyticsIntent = ConsentStatus.Granted, AdsIntent = ConsentStatus.Denied });

                RemoteConfigService.Instance.FetchCompleted += OnConfigsFetched;
                RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
            }
            catch (Exception e)
            {
                Debug.LogError($"Init failed: {e.Message}");
                ApplyDefault();
            }
            while (_running)
            {
                await UniTask.Yield();
            }
            await UniTask.NextFrame();
        }
        public float GetWindowAnimationSpeed()
        {
            return _uIAnimationSpeed;
        }

        private void OnConfigsFetched(ConfigResponse response)
        {
            _uIAnimationSpeed = RemoteConfigService.Instance.appConfig.GetFloat(Constants.UIAnimationSpeedKey, Constants.DefaultWindowAnimationSpeed);

            if (response.status == ConfigRequestStatus.Success)
                Debug.Log($"Success (origin: {response.requestOrigin}): {_uIAnimationSpeed}");
            else
                Debug.Log($"Failed/Cached (origin: {response.requestOrigin}): {_uIAnimationSpeed}");

            _running = false;
        }

        private void ApplyDefault()
        {
            _uIAnimationSpeed = RemoteConfigService.Instance.appConfig.GetFloat(Constants.UIAnimationSpeedKey, Constants.DefaultWindowAnimationSpeed);
            Debug.Log($"Using pure default: {_uIAnimationSpeed}");
            _running = false;
        }
    }
}