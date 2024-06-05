using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetWorkingManager: NetworkManager
{ 
    [SerializeField] Login _login;

    //ȣ��Ʈ�� �̸��� ����ɶ� ȣ��Ǵ� �Լ�
    public void OnInputpPlayerName_SetHostName(string hostName)
    { 
      this.networkAddress = hostName;
    }

    //�������� ������ �������� ȣ��
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        
    }

    //Ŭ���̾�Ʈ�� ������ �������� ȣ��
    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        base.OnClientConnect();

        if (_login != null)
        {
            _login.SetUIclient();//������ ���������� UI
        }
    }


}
