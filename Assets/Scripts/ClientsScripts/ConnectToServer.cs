using Configs;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Services.Interface;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VContainer;

namespace ClientsScripts
{
    public class ConnectToServer : MonoBehaviour
    {
        [SerializeField] private Button _buttonConnect;
        private SmartFox _sfs;
        private const string SERVER_URL = "https://apisfs.ru:9443/getSfsConfig";
        private IEncryptionService _encryptionService;

        private LoginClient _loginClient;

        [Inject]
        private void Construct(IEncryptionService encryptionService, SmartFox sfs)
        {
            _sfs = sfs;
            _encryptionService = encryptionService;
        }

        private void Start()
        {
            _buttonConnect.OnClickAsObservable().Subscribe(_ => Connection().Forget()).AddTo(this);
        }

        private void Update()
        {
            _sfs?.ProcessEvents();
        }
    
        private void OnApplicationQuit()
        {
            if(_sfs is { IsConnected: true }) 
                _sfs.Disconnect();
        }

        private async UniTaskVoid Connection()
        {
            _buttonConnect.interactable = false;
            Debug.Log("Now connecting...");
        
            _sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
            _sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
            _sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
            
            var gameServerData = await ConnectionToWebApi(SERVER_URL);

            var configData = new ConfigData
            {
                Host = gameServerData.Host,
                Port = gameServerData.Port,
                Zone = gameServerData.Zone
            };
            
            _loginClient = new LoginClient(_sfs, gameServerData);

            _loginClient.SubscribeListeners();
            _sfs.Connect(configData);
        }

        private void OnExtensionResponse(BaseEvent evt)
        {
            var response = (SFSObject)evt.Params["params"];
            var result = response.GetInt("sum");
            
            Debug.Log($"Response from server: {result}");
        }

        private async UniTask<GameServerData> ConnectionToWebApi(string url)
        {
            using var client = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            client.downloadHandler = new DownloadHandlerBuffer();
            var operation = client.SendWebRequest();

            while (!operation.isDone)
            {
                await UniTask.Yield();
            }

            if (client.result == UnityWebRequest.Result.Success)
            {
                var result = _encryptionService.Decrypt(client.downloadHandler.text);
                return JsonConvert.DeserializeObject<GameServerData>(result);
            }
            
            Debug.LogError($"Error while connecting to {url}: {client.error}");
            return null;
        }

        private void OnConnectionLost(BaseEvent evt)
        {
            Debug.LogWarning("Connection Lost; is reason is: " + (string)evt.Params["reason"]);
            ResetConnect();
        }

        private void OnConnection(BaseEvent evt)
        {
            if ((bool)evt.Params["success"])
            {
                Debug.Log("Connection established successfully");
                Debug.Log("SFS2X API version: " + _sfs.Version);
                
                _loginClient.Login();
            }
            else
            {
                Debug.LogError("Connection failed; is the server running at all?");
                ResetConnect();
            }
        }

        private void ResetConnect()
        {
            _sfs.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
            _sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);

            _sfs = null;
            _buttonConnect.interactable = true;
        }
    }
}
