using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


//네트워킹 인증자
public partial class NetworkingAuthenticator : NetworkAuthenticator
{
    //해시셋(검색 , 삽입, 삭제의 유용한 저장 장치)
    //인증 대기주인 연결들을 저장하는 해시셋
    // _connectionsPandingDisconnect : 연결끊기 보류
    //NetworkConnection : 네트워크 연결
    readonly HashSet<NetworkConnection> _connectionsPendingDisconnect = new HashSet<NetworkConnection>();

    //고유 플레이어의 이름을 저장하는 해시셋
    internal static readonly HashSet<string> _playerNames = new HashSet<string>();

    //클라이언트가 인증 요청 시 보낼 메세지 
    public struct AuthReqMsg : NetworkMessage 
    {
        public string authuserName;
    }

    //서버가 인증 응답시 보낼 메세지
    public struct AuthResMsg : NetworkMessage
    {
        public byte code;
        public string msg;
    }

    #region ServerSide
    [UnityEngine.RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    { 
      //정적변수를 초기화 하는 메서드

    }

    public override void OnStartServer()
    {
        //클라로부터 인증처리를 위한 핸들러 연결
        NetworkServer.RegisterHandler<AuthReqMsg>(OnAuthRequestMessage, false);
    }

    public override void OnStopServer()
    {
        //서버중지 시 인증 응답 핸들러 들록 해제 
        NetworkServer.UnregisterHandler<AuthResMsg>();
    }
    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
    {
    }

    public void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthReqMsg msg)
    {
        //클라이언트로부터 인증 요청 메세지가 도착했을떄 호출

        Debug.Log($"인증요청 : {msg.authuserName}");

        //이미 인증 대기 중인 연결이면 처리 중지
        if (_connectionsPendingDisconnect.Contains(conn)) return;

        //외부 서버나 DB를 호출하여 인증 확인 (간단한 예제는 플레이어 이름 중복 검사)
        if (!_playerNames.Contains(msg.authuserName))
        {
            _playerNames.Add(msg.authuserName);

            //인증데이터 설정
            conn.authenticationData = msg.authuserName;

            //성공 응담 메세지 생성및 전송
            AuthResMsg authReqMsg = new AuthResMsg
            {
                code = 100,
                msg = "Auth Success"

            };
            conn.Send(authReqMsg);
            ServerReject(conn);
        }
        else
        { 
          _connectionsPendingDisconnect.Add(conn);

            AuthResMsg authResMsg = new AuthResMsg
            {
                code = 200,
                msg = "이름이 이미 있습니다! 다시 시도하세요"

            };
            conn.Send(authResMsg);
            conn.isAuthenticated = false;

            StartCoroutine(DelayedDisconnect(conn, 1.0f));
        }
        
    }

    IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float waitTime)
    {
        //일정 시간 대기 후 연결 해제
        yield return new WaitForSeconds(waitTime);
        ServerReject(conn);//서버에서 인증 실패 처리

        yield return null;
        _connectionsPendingDisconnect.Remove(conn);//연결해제 후 대기중인 연결 목록에서 제거 
    }
    #endregion
}