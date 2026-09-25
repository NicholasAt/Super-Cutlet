using Cysharp.Threading.Tasks;
using System;

namespace CodeBase.Services.RemoteConfig
{
    public interface IRemoteConfigService : IService
    {
        UniTask Check();
        float GetWindowAnimationSpeed();
    }
}