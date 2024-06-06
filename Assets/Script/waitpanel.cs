using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class waitpanel : NetworkBehaviour
{
    [SerializeField] GameObject ServerInfoPrefab; // 서버 정보 오브젝트 프리팹
    [SerializeField] Transform ServerInfoParent; // 서버 정보 부모 트랜스폼

    [SerializeField] GameObject ClientInfoPrefab; // 클라이언트 정보 오브젝트 프리팹
    [SerializeField] Transform ClientInfoParent; // 클라이언트 정보 부모 트랜스폼

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
    }

    // 클라이언트에서 서버로 호출(권한 없이도 명령을 보낼 수 있음)
    [Command(requiresAuthority = false)]
    void CmdSendName(string name, NetworkConnectionToClient sender = null)
    {
        if (!_connectedNameDic.ContainsKey(sender))
        {
            _connectedNameDic[sender] = name;

            // 클라이언트 정보 오브젝트 생성 및 설정
            GameObject clientInfoObj = Instantiate(ClientInfoPrefab, ClientInfoParent);
            NetworkServer.Spawn(clientInfoObj);

            // 생성된 오브젝트의 자식 텍스트 컴포넌트를 찾아 클라이언트 이름으로 설정
            RpcSetPlayerName(clientInfoObj, name);
        }
    }

    // 모든 클라이언트에서 호출되는 메서드
    [ClientRpc]
    void RpcSetPlayerName(GameObject playerInfoObj, string playerName)
    {
        Text playerNameText = playerInfoObj.GetComponentInChildren<Text>();
        if (playerNameText != null)
        {
            playerNameText.text = playerName;
        }
    }

    // UI에서 호출하여 이름을 서버로 보냄
    public void SendernameToServer(string playerName)
    {
        if (isClient)
        {
            CmdSendName(playerName);
        }
    }
}
