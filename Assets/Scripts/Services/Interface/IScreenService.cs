using Cysharp.Threading.Tasks;
using UI.Base;

namespace Services.Interface
{
    public interface IScreenService
    {
        UniTask<TScreen> OpenAsync<TScreen>() where TScreen : View;
        void OpenLoading<TScreen>() where TScreen : View;
        void CloseLoading();
        void Close();
    }
}