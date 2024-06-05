using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Start : NetworkBehaviour
{

    [SerializeField] GameObject PlayerInfoPrefab;//클라이언트 정보 오브젝트
    [SerializeField] Transform PlayerInfoParent;// // 의 부모
    

   internal static string _hostPlayerName;

    //서버 온리 - 연결된 플레이어의 이름
    internal static readonly Dictionary <NetworkConnectionToClient, string> _connectedNameDic = new Dictionary<NetworkConnectionToClient, string> ();

    //플레이어의 이름 설정
    public void SetLocalPlayername(string playerName)
    { 
       _hostPlayerName = playerName;
    }

    //서버시작
    public override void OnStartServer()
    { 
        this.gameObject.SetActive (true);
       

    }

    //클라이언트 시작
    public override void OnStartClient()
    {
        this.gameObject.SetActive(true);
      
    }




    //클라이언트에서 서버로 호출(권한 없이도 명령을 보낼 수 있음)
    [Command(requiresAuthority = false)]
    //sender : 명령을 보낸 클라이언트의 연결정보
    void CmdSendName(string name, NetworkConnectionToClient sender = null)
    {
        //클라이언트가 없으면 추가
        if (!_connectedNameDic.ContainsKey(sender))
        {
            //클라이언트의 이름을 딕셔너리에 추가
            _connectedNameDic[sender] = name;
        
        }
    
    }

    //UI에서 호출하여 이름을 서버로 보냄
    public void SendernameToSarver(string playerName)
    {
        if (isClient)
        {
            //서버로 이름을 보내는 명령
            GarticUser localPlayer = NetworkClient.connection.identity.GetComponent<GarticUser>();
            if (localPlayer != null) 
            {
               localPlayer.name = playerName;
            }
        }


    
    }
}
