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


    public void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthReqMsg msg)
    {
        // 클라이언트로부터 인증 요청 메시지가 도착했을 때 호출
        Debug.Log($"인증 요청: {msg.authuserName}");

        // 이미 인증 대기 중인 연결이면 처리 중지
        if (_connectionsPendingDisconnect.Contains(conn)) return;

        // 외부 서버나 DB를 호출하여 인증 확인 (간단한 예제는 플레이어 이름 중복 검사)
        if (!_playerNames.Contains(msg.authuserName))
        {
            // 중복되지 않은 경우
            _playerNames.Add(msg.authuserName);

            // 인증 데이터 설정
            conn.authenticationData = msg.authuserName;

            // 성공 응답 메시지 생성 및 전송
            AuthResMsg authReqMsg = new AuthResMsg
            {
                code = 100,
                msg = "인증성공"
            };
            conn.Send(authReqMsg);
           ServerAccept(conn);
        }
        else
        {
            // 중복된 경우
            _connectionsPendingDisconnect.Add(conn);

            // 실패 응답 메시지 생성 및 전송
            AuthResMsg authResMsg = new AuthResMsg
            {
                code = 200,
                msg = "이미 사용 중인 이름입니다. 다른 이름을 선택해주세요."
            };
            conn.Send(authResMsg);
            conn.isAuthenticated = false;

            // 지연 후 연결 해제 처리하는 코루틴
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