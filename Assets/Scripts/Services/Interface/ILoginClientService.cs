using R3;

namespace Services.Interface
{
    public interface ILoginClientService
    {
        void Login(string login, string password);
        ReadOnlyReactiveProperty<string> LoginErrorRequest { get; }
    }
}