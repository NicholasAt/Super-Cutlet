using CodeBase.Services;
using CodeBase.UI.Windows;
using Cysharp.Threading.Tasks;

namespace CodeBase.UI.Services.Window
{
    public interface IWindowService : IService
    {
        UniTask Open(WindowId id);
        void Close(WindowId id);
        bool GetWindow<TWindow>(WindowId id, out TWindow window) where TWindow : BaseWindow;
    }
}