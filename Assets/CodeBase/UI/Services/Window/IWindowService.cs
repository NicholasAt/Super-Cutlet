using CodeBase.Services;
using CodeBase.UI.Windows;

namespace CodeBase.UI.Services.Window
{
    public interface IWindowService : IService
    {
        void Open(WindowId id);

        void Close(WindowId id);

        bool GetWindow<TWindow>(WindowId id, out TWindow window) where TWindow : BaseWindow;
    }
}