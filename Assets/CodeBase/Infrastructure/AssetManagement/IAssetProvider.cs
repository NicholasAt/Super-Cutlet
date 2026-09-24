using CodeBase.Services;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 at);
        UniTask<T> LoadAsync<T>(AssetReference reference, CancellationToken ct = default);
        void ReleaseAll();
    }
}