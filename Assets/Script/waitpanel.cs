using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class waitpanel : NetworkBehaviour
{
    [SerializeField] GameObject ServerInfoPrefab; // ���� ���� ������Ʈ ������
    [SerializeField] Transform ServerInfoParent; // ���� ���� �θ� Ʈ������

    [SerializeField] GameObject ClientInfoPrefab; // Ŭ���̾�Ʈ ���� ������Ʈ ������
    [SerializeField] Transform ClientInfoParent; // Ŭ���̾�Ʈ ���� �θ� Ʈ������

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
    }

    // Ŭ���̾�Ʈ���� ������ ȣ��(���� ���̵� ����� ���� �� ����)
    [Command(requiresAuthority = false)]
    void CmdSendName(string name, NetworkConnectionToClient sender = null)
    {
        if (!_connectedNameDic.ContainsKey(sender))
        {
            _connectedNameDic[sender] = name;

            // Ŭ���̾�Ʈ ���� ������Ʈ ���� �� ����
            GameObject clientInfoObj = Instantiate(ClientInfoPrefab, ClientInfoParent);
            NetworkServer.Spawn(clientInfoObj);

            // ������ ������Ʈ�� �ڽ� �ؽ�Ʈ ������Ʈ�� ã�� Ŭ���̾�Ʈ �̸����� ����
            RpcSetPlayerName(clientInfoObj, name);
        }
    }

    // ��� Ŭ���̾�Ʈ���� ȣ��Ǵ� �޼���
    [ClientRpc]
    void RpcSetPlayerName(GameObject playerInfoObj, string playerName)
    {
        Text playerNameText = playerInfoObj.GetComponentInChildren<Text>();
        if (playerNameText != null)
        {
            playerNameText.text = playerName;
        }
    }

    // UI���� ȣ���Ͽ� �̸��� ������ ����
    public void SendernameToServer(string playerName)
    {
        if (isClient)
        {
            CmdSendName(playerName);
        }
    }
}
