using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkingAuthenticator
{
    [SerializeField] Login _login;

    [Header("Client Username")]
    public string _playerName;

    //사용자 이름 입력 필드의 값이 변경될 때 호출
    public void OnInputValueChaged_SetPlayerName(string userName)
    { 
      _playerName = userName;
        _login.SetUIChanged();
    }

    //클라이언트가 시작될 때 호출
    public override void OnStartClient()
    {
        NetworkClient.RegisterHandler<AuthResMsg>(OnAuthResponseMessage, false);ㅁ
    }

    //클라이언트가 중지 될때 호출
    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<AuthReqMsg>();
    }

    //클라이언트가 인증 요청을 보낼때 호출
    public override void OnClientAuthenticate()
    {
        NetworkClient.Send(new AuthReqMsg { authuserName = _playerName });
    }

    //서버로부터 인증 응답 메세지를 받았을 때 호출
    public void OnAuthResponseMessage(AuthResMsg msg)
    {
        if (msg.code == 100)//인증성공
        {
            Debug.Log($"소환 :{msg.code}{msg.msg}");
             ClientAccept();//클라이언트 인증 성공 처리
        }
        else
        {
            Debug.LogError($"소환 :{msg.code}{msg.msg}");
            NetworkManager.singleton.StopHost();

            _login.SetUIError(msg.msg);
        }
    }

}
