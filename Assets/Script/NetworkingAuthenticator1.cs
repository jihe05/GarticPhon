using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkingAuthenticator
{
    [SerializeField] Login _login;

    [Header("Client Username")]
    public string _playerName;

    //����� �̸� �Է� �ʵ��� ���� ����� �� ȣ��
    public void OnInputValueChaged_SetPlayerName(string userName)
    { 
      _playerName = userName;
        _login.SetUIChanged();
    }

    //Ŭ���̾�Ʈ�� ���۵� �� ȣ��
    public override void OnStartClient()
    {
        NetworkClient.RegisterHandler<AuthResMsg>(OnAuthResponseMessage, false);��
    }

    //Ŭ���̾�Ʈ�� ���� �ɶ� ȣ��
    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<AuthReqMsg>();
    }

    //Ŭ���̾�Ʈ�� ���� ��û�� ������ ȣ��
    public override void OnClientAuthenticate()
    {
        NetworkClient.Send(new AuthReqMsg { authuserName = _playerName });
    }

    //�����κ��� ���� ���� �޼����� �޾��� �� ȣ��
    public void OnAuthResponseMessage(AuthResMsg msg)
    {
        if (msg.code == 100)//��������
        {
            Debug.Log($"��ȯ :{msg.code}{msg.msg}");
             ClientAccept();//Ŭ���̾�Ʈ ���� ���� ó��
        }
        else
        {
            Debug.LogError($"��ȯ :{msg.code}{msg.msg}");
            NetworkManager.singleton.StopHost();

            _login.SetUIError(msg.msg);
        }
    }

}
