using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;


public class TextUI : NetworkBehaviour
{

    public Image fillImage;
    public float fillcount = 30;

    public InputField inputField;

    public GameObject drowingObj;

    // ��ư�� Ŭ���� �÷��̾ �����ϴ� �ؽü�
    private static HashSet<NetworkConnectionToClient> playersWhoClickedButton = new HashSet<NetworkConnectionToClient>();
    private static int totalPlayers;

    void OnEnable()
    {
        NetworkManager.singleton.OnServerAddPlayer += OnServerAddPlayer;
        NetworkManager.singleton.OnServerDisconnect += OnServerDisconnect;


    }

    void OnDisable()
    {
        NetworkManager.singleton.OnServerAddPlayer -= OnServerAddPlayer;
        NetworkManager.singleton.OnServerDisconnect -= OnServerDisconnect;
    }

    void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        totalPlayers = NetworkServer.connections.Count;
        Debug.Log("Player connected. Total players: " + totalPlayers);
    }

    void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        totalPlayers = NetworkServer.connections.Count;
        Debug.Log("Player disconnected. Total players: " + totalPlayers);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        totalPlayers = NetworkServer.connections.Count; // �������� �� �÷��̾� �� ����
      
    }

    public void Start()
    {

        StartCoroutine(TimeCount());

    }


    //Ÿ�̸�
    IEnumerator TimeCount()
    {
        float elapsedTime = 0f;
        float startFillAmount = fillImage.fillAmount;

        while (elapsedTime < fillcount)
        {
            elapsedTime += Time.deltaTime;
            fillImage.fillAmount = Mathf.Lerp(startFillAmount, 0f, elapsedTime / fillcount);

            yield return null;
        }


        fillImage.fillAmount = 0f;
        // Ÿ�̸Ӱ� 0�ʰ� �Ǿ��� �� RPC ȣ��
        CmdTimerEnded();

    }

    public void OnSaveButtonClick()
    {
        Debug.Log("Total players: " + totalPlayers);
        SaveInput(); // �Է� ������ ����
        CmdPlayerClickedButton(); // ������ ��� ����
    }

    // ������ Ÿ�̸� ���� ��� ���� (�������� �����)
    [Command(requiresAuthority = false)]
    private void CmdTimerEnded(NetworkConnectionToClient sender = null)
    {
        RpcDisableObject(); // ��� Ŭ���̾�Ʈ�� ������Ʈ ��Ȱ��ȭ ȣ��
    }

    // InputField ������ PlayerPrefs�� ����
    private void SaveInput()
    {
        string inputValue = inputField.text; // �Է� �ʵ��� ������ ������
        PlayerPrefs.SetString("SavedInput", inputValue); // PlayerPrefs�� �� ����
        PlayerPrefs.Save(); // ����� ���� ��ũ�� ���
        Debug.Log("Input saved: " + inputValue);
    }

    // ������ ��� ���� (����� �������� �����)
    [Command(requiresAuthority = false)]
    private void CmdPlayerClickedButton(NetworkConnectionToClient sender = null)
    {

        Debug.Log("�÷��̾� �� Ȯ��");
        if (sender != null)
        {
            if (!playersWhoClickedButton.Contains(sender))
            {
                playersWhoClickedButton.Add(sender); // ��ư�� Ŭ���� �÷��̾� �߰�
            }

            // ��� �÷��̾ ��ư�� Ŭ���ߴ��� Ȯ��
            if (playersWhoClickedButton.Count >= totalPlayers)
            {
                Debug.Log("��� �÷��̾ ��ư�� Ŭ����");
                RpcDisableObject(); // ��� Ŭ���̾�Ʈ�� ������Ʈ ��Ȱ��ȭ ȣ��
            }
        }
    }

    // Ŭ���̾�Ʈ���� Ư�� ������Ʈ ��Ȱ��ȭ
    [ClientRpc]
    private void RpcDisableObject()
    {
        if (drowingObj != null)
        {
            drowingObj.SetActive(false); // ������Ʈ ��Ȱ��ȭ
            Debug.Log("Target object disabled");
        }
        else
        {
            Debug.LogWarning("null");
        }
    }

}
