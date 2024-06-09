using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class waitpanel : NetworkBehaviour
{
    public GameObject playerPrefab;
    public Transform playerParent;
    public GameObject startGame;
    [SerializeField] internal GameObject Client_btn;

    internal static string _hostPlayerName;
    internal static readonly Dictionary<NetworkConnectionToClient, string> _connectedNameDic = new Dictionary<NetworkConnectionToClient, string>();

    public static waitpanel Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    // 모든 클라이언트에게 프리팹 추가 사항 전파
    [ClientRpc]
    public void SetLocalPlayerName(string playername)
    {

        _hostPlayerName = playername;
        // 프리팹 추가
        GameObject prefabInstance = Instantiate(playerPrefab, playerParent);

        // 생성된 프리팹의 이름 설정
        prefabInstance.GetComponentInChildren<Text>().text = playername;
        
    }

    // 클라이언트에서 서버로 호출 (권한 없이 명령을 보낼 수 있음)
    [Command(requiresAuthority = false)]
    void CmdSendName(string name, NetworkConnectionToClient sender = null)
    {
        if (sender != null && !_connectedNameDic.ContainsKey(sender))
        {
            _connectedNameDic.Add(sender, name);

            RpcSetPlayerName(playerPrefab, name);
        }
    }

    // 모든 클라이언트에서 호출되는 메서드
    [ClientRpc]
    void RpcSetPlayerName(GameObject playerInfoObj, string playerName)
    {
        if (playerInfoObj != null)
        {
            Text playerNameText = playerInfoObj.GetComponentInChildren<Text>();
            if (playerNameText != null)
            {
                playerNameText.text = playerName;
                Debug.Log($"Player name set to: {playerName}");
            }
        }
    }


    // UI에서 호출하여 이름을 서버로 보냄
    public void SenderNameToServer(string playerName)
    {
        if (isLocalPlayer)
        {

            CmdSendName(playerName);
        }
    }

    // 클라이언트 시작
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameObject.SetActive(true);
        _connectedNameDic.Clear();
        SenderNameToServer((string)connectionToClient.authenticationData);
        Client_btn.SetActive(false);


    }

    // 서버 시작
    public override void OnStartServer()
    {
        base.OnStartServer();
        gameObject.SetActive(true);
        _connectedNameDic.Clear();
        
    }

    // 나가기
    public void OnClick_Exit()
    {
        NetworkManager.singleton.StopHost();
    }


    [ClientRpc]
    public void Btn_GameStart()
    {
        this.gameObject.SetActive(false);
        startGame.SetActive(true);
       
    }

    

}

