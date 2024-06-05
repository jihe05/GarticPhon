using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetWorkingManager: NetworkManager
{ 
    [SerializeField] Login _login;

    //호스트의 이름이 변경될때 호출되는 함수
    public void OnInputpPlayerName_SetHostName(string hostName)
    { 
      this.networkAddress = hostName;
    }

    //서버에서 연결이 끊겼을때 호출
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        
    }

    //클라이언트가 연결이 끊겼을때 호출
    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        base.OnClientConnect();

        if (_login != null)
        {
            _login.SetUIclient();//연결이 끊겼을때의 UI
        }
    }


}
