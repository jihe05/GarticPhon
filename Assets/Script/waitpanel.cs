using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class waitpanel : NetworkBehaviour
{
    [SerializeField] GameObject ServerInfoPrefab; // 서버 정보 오브젝트 프리팹
    [SerializeField] Transform ServerInfoParent; // 서버 정보 부모 트랜스폼


    internal static string _hostPlayerName;
    internal static readonly Dictionary<NetworkConnectionToClient, string> _connectedNameDic = new Dictionary<NetworkConnectionToClient, string>();

    public static waitpanel Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
       
    }

    // 플레이어의 이름 설정
    public void SetLocalPlayername(string playerName)
    {
        _hostPlayerName = playerName;
     
        // 서버 정보 오브젝트 생성 및 이름 설정
        GameObject serverInfoObj = Instantiate(ServerInfoPrefab, ServerInfoParent);
        serverInfoObj.GetComponentInChildren<Text>().text = playerName;
        serverInfoObj.GetComponentInChildren<Image[]>();
    }

    //시작 호스트
    public override void OnStartServer()
    {
        this.gameObject.SetActive(true);
        _connectedNameDic.Clear();
    }

    //시작 클라이언트
    public override void OnStartClient()
    {
        this.gameObject.SetActive(true);
        _connectedNameDic.Clear();
        SetLocalPlayername((string)connectionToClient.authenticationData);
        SendernameToServer((string)connectionToClient.authenticationData);
    }

    // 클라이언트에서 서버로 호출(권한 없이도 명령을 보낼 수 있음)
    [Command(requiresAuthority = false)]
    void CmdSendName(string name, NetworkConnectionToClient sender = null)
    {
        if (!_connectedNameDic.ContainsKey(sender))
        {
            Debug.Log(1);
            var player = sender.identity.GetComponent<GarticUser>();
            var playerName = player.PlayerName;
            _connectedNameDic.Add(sender,name);
        
        }
        
    }

    //나가기
    public void onClick_Exit()
    { 
        NetworkManager.singleton.StopHost();
      
    }

    // 모든 클라이언트에서 호출되는 메서드
    [ClientRpc]
    void RpcSetPlayerName(GameObject playerInfoObj, string playerName)
    {
        Text playerNameText = playerInfoObj.GetComponentInChildren<Text>();
        Debug.Log(playerNameText);
        if (playerNameText != null)
        {
            playerNameText.text = playerName;
        }
    }

    // UI에서 호출하여 이름을 서버로 보냄
    public void SendernameToServer(string playerName)
    {
        if (!isLocalPlayer)
        {
            CmdSendName(playerName);
        }
        
    }
}
