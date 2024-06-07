using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class waitpanel : NetworkBehaviour
{
    [SerializeField] GameObject ServerInfoPrefab; // ���� ���� ������Ʈ ������
    [SerializeField] Transform ServerInfoParent; // ���� ���� �θ� Ʈ������


    internal static string _hostPlayerName;
    internal static readonly Dictionary<NetworkConnectionToClient, string> _connectedNameDic = new Dictionary<NetworkConnectionToClient, string>();

    public static waitpanel Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
       
    }

    // �÷��̾��� �̸� ����
    public void SetLocalPlayername(string playerName)
    {
        _hostPlayerName = playerName;
     
        // ���� ���� ������Ʈ ���� �� �̸� ����
        GameObject serverInfoObj = Instantiate(ServerInfoPrefab, ServerInfoParent);
        serverInfoObj.GetComponentInChildren<Text>().text = playerName;
        serverInfoObj.GetComponentInChildren<Image[]>();
    }

    //���� ȣ��Ʈ
    public override void OnStartServer()
    {
        this.gameObject.SetActive(true);
        _connectedNameDic.Clear();
    }

    //���� Ŭ���̾�Ʈ
    public override void OnStartClient()
    {
        this.gameObject.SetActive(true);
        _connectedNameDic.Clear();
        SetLocalPlayername((string)connectionToClient.authenticationData);
        SendernameToServer((string)connectionToClient.authenticationData);
    }

    // Ŭ���̾�Ʈ���� ������ ȣ��(���� ���̵� ����� ���� �� ����)
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

    //������
    public void onClick_Exit()
    { 
        NetworkManager.singleton.StopHost();
      
    }

    // ��� Ŭ���̾�Ʈ���� ȣ��Ǵ� �޼���
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

    // UI���� ȣ���Ͽ� �̸��� ������ ����
    public void SendernameToServer(string playerName)
    {
        if (!isLocalPlayer)
        {
            CmdSendName(playerName);
        }
        
    }
}
