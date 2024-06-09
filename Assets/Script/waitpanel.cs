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


    // ��� Ŭ���̾�Ʈ���� ������ �߰� ���� ����
    [ClientRpc]
    public void SetLocalPlayerName(string playername)
    {

        _hostPlayerName = playername;
        // ������ �߰�
        GameObject prefabInstance = Instantiate(playerPrefab, playerParent);

        // ������ �������� �̸� ����
        prefabInstance.GetComponentInChildren<Text>().text = playername;
        
    }

    // Ŭ���̾�Ʈ���� ������ ȣ�� (���� ���� ����� ���� �� ����)
    [Command(requiresAuthority = false)]
    void CmdSendName(string name, NetworkConnectionToClient sender = null)
    {
        if (sender != null && !_connectedNameDic.ContainsKey(sender))
        {
            _connectedNameDic.Add(sender, name);

            RpcSetPlayerName(playerPrefab, name);
        }
    }

    // ��� Ŭ���̾�Ʈ���� ȣ��Ǵ� �޼���
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


    // UI���� ȣ���Ͽ� �̸��� ������ ����
    public void SenderNameToServer(string playerName)
    {
        if (isLocalPlayer)
        {

            CmdSendName(playerName);
        }
    }

    // Ŭ���̾�Ʈ ����
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameObject.SetActive(true);
        _connectedNameDic.Clear();
        SenderNameToServer((string)connectionToClient.authenticationData);
        Client_btn.SetActive(false);


    }

    // ���� ����
    public override void OnStartServer()
    {
        base.OnStartServer();
        gameObject.SetActive(true);
        _connectedNameDic.Clear();
        
    }

    // ������
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

