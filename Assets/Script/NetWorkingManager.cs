using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class NetWorkingManager : NetworkManager
{
    [SerializeField] Login _login;
    [SerializeField] waitpanel waitpanel;

    // 호스트의 이름이 변경될 때 호출되는 함수
    public void OnInputPlayerName_SetHostName(string hostName)
    {
        this.networkAddress = hostName;
    }

    // 서버에서 연결이 끊겼을 때 호출
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (waitpanel != null && waitpanel._connectedNameDic.ContainsKey(conn))
        {
            var disconnectedName = waitpanel._connectedNameDic[conn];
            
            waitpanel._connectedNameDic.Remove(conn);
            waitpanel._hostPlayerName = string.Empty; // 클라이언트 이름 제거

            // 연결이 끊긴 클라이언트의 오브젝트 제거 등 필요한 처리 추가
        }

        base.OnServerDisconnect(conn);
    }

    // 클라이언트가 연결이 끊겼을 때 호출
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        if (_login != null)
        {
            _login.SetUIclient(); // 연결이 끊겼을 때의 UI
        }
    }
}
