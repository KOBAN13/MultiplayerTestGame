using Cysharp.Threading.Tasks;
using UI.Base;
using VContainer;

namespace Services.Interface
{
    public interface IScreenService
    {
        UniTask<TScreen> OpenAsync<TScreen>() where TScreen : View;
        void OpenLoading<TScreen>() where TScreen : View;
        void CloseLoading();
        void CloseScreen<TScreen>() where TScreen : View;
        void Close();
    }
}