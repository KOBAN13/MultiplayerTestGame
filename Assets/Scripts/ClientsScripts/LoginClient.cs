using Configs;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;

namespace ClientsScripts
{
    public class LoginClient
    {
        public User User { get; private set; }

        private readonly SmartFox _sfs;
        private readonly GameServerData _gameServerData;
        
        public LoginClient(SmartFox sfs, GameServerData gameServerData)
        {
            _sfs = sfs;
            _gameServerData = gameServerData;
        }

        public void SubscribeListeners()
        {
            _sfs.AddEventListener(SFSEvent.LOGIN, OnLoginSuccess);
            _sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        }

        public void Login()
        {
            _sfs.Send(new LoginRequest("", "", _gameServerData.Zone));
        }

        private void OnLoginSuccess(BaseEvent evt)
        {
            User = (User)evt.Params["user"];
            Debug.Log($"Login successful; username is {User.Name}");

            var parameters = SFSObject.NewInstance();
            
            parameters.PutInt("n1", 5);
            parameters.PutInt("n2", 6);
            
            _sfs.Send(new ExtensionRequest("math", parameters));
        }

        private void OnLoginError(BaseEvent evt)
        {
            Debug.LogWarning($"Login failed: {(string)evt.Params["errorMessage"]}" );
        }
    }
}