using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Logic
{
    public class SceneLoader
    {
        public async UniTask LoadSingle(string key, Action onLoaded)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(key, LoadSceneMode.Single);
            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                onLoaded?.Invoke();
            }
            else
            {
                Debug.LogError($"cant load");
                if (handle.IsValid())
                    Addressables.Release(handle);
            }
        }
    }
}