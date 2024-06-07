using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class NetWorkingManager : NetworkManager
{
    [SerializeField] Login _login;
    [SerializeField] waitpanel waitpanel;

    // ȣ��Ʈ�� �̸��� ����� �� ȣ��Ǵ� �Լ�
    public void OnInputPlayerName_SetHostName(string hostName)
    {
        this.networkAddress = hostName;
    }

    // �������� ������ ������ �� ȣ��
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (waitpanel != null && waitpanel._connectedNameDic.ContainsKey(conn))
        {
            var disconnectedName = waitpanel._connectedNameDic[conn];
            
            waitpanel._connectedNameDic.Remove(conn);
            waitpanel._hostPlayerName = string.Empty; // Ŭ���̾�Ʈ �̸� ����

            // ������ ���� Ŭ���̾�Ʈ�� ������Ʈ ���� �� �ʿ��� ó�� �߰�
        }

        base.OnServerDisconnect(conn);
    }

    // Ŭ���̾�Ʈ�� ������ ������ �� ȣ��
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        if (_login != null)
        {
            _login.SetUIclient(); // ������ ������ ���� UI
        }
    }
}
