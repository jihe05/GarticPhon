using System.Collections;
using UnityEngine;
using Mirror;

//서버와 클라이언트간에 동기화하고 , 플레이어 이름을 UI에 설정 
public class GarticUser : NetworkBehaviour
{
    // SyncVar - 서버 변수를 모든 클라에 자동 동기화하는데 사용됨
    // 클라가 직접 변경하면 안되고, 서버에서 변경해야 함
    [SyncVar]
    public string PlayerName;

    //서버에서 플레이어가 연결될때 호출 되는 함수
    public override void OnStartServer()
    {
        //서버에서 플레이어가 연결될때 Playername을 인증 데이터로 사용
        PlayerName = (string)connectionToClient.authenticationData;
    }

    //로컬 메서드는 오직 서버에서만 호출
    public override void OnStartLocalPlayer()
    {
        //Startpanel오브젝트를 찾아 플레이어의 이름을 UI에 설정
        var objStartUI = GameObject.Find("Start Panel");

        if (objStartUI != null)
        {
            //Start스크립트의 컴포넌트를 가져와 
            var StartPanel = objStartUI.GetComponent<waitpanel>();
            if (StartPanel != null)
            {
                //플레이어의 이름을 UI의 설정
                StartPanel.SetLocalPlayername(PlayerName);
            }
        }

    }
    [Command(requiresAuthority = false)]
    public void CodSetplayerName(string newName)
    { 
      PlayerName=newName;
    }
}
