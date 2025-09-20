using Configs;
using Helpers;
using R3;
using Services.Interface;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Unity.VisualScripting;
using VContainer;

namespace Services.Connections
{
    public class LoginClientService : ILoginClientService, IInitializable
    {
        private User _user;

        private readonly SmartFox _sfs;
        private readonly ReactiveProperty<string> _loginErrorRequest = new();
        
        public ReadOnlyReactiveProperty<string> LoginErrorRequest => _loginErrorRequest;
        
        [Inject]
        public LoginClientService(SmartFox sfs)
        {
            _sfs = sfs;
        }
        
        public void Initialize()
        {
            _sfs.AddEventListener(SFSResponseHelper.LOGIN_RESULT, OnLoginResult);
        }

        public void Login(string login, string password)
        {
            _sfs.Send(new LoginRequest(login, password));
        }
        
        private void OnLoginResult(BaseEvent evt)
        {
            var status = (bool)evt.Params[SFSResponseHelper.OK];

            if (status)
            {
                var userName = (string)evt.Params[SFSResponseHelper.USER_NAME];
                _user = (User)evt.Params["user"];
            }
            else
            {
                var errorMessage = (string)evt.Params[SFSResponseHelper.ERROR];
            }
        }
    }
}