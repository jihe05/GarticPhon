using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


//��Ʈ��ŷ ������
public partial class NetworkingAuthenticator : NetworkAuthenticator
{
    //�ؽü�(�˻� , ����, ������ ������ ���� ��ġ)
    //���� ������� ������� �����ϴ� �ؽü�
    // _connectionsPandingDisconnect : ������� ����
    //NetworkConnection : ��Ʈ��ũ ����
    readonly HashSet<NetworkConnection> _connectionsPendingDisconnect = new HashSet<NetworkConnection>();

    //���� �÷��̾��� �̸��� �����ϴ� �ؽü�
    internal static readonly HashSet<string> _playerNames = new HashSet<string>();

    //Ŭ���̾�Ʈ�� ���� ��û �� ���� �޼��� 
    public struct AuthReqMsg : NetworkMessage 
    {
        public string authuserName;
    }

    //������ ���� ����� ���� �޼���
    public struct AuthResMsg : NetworkMessage
    {
        public byte code;
        public string msg;
    }

    #region ServerSide
    [UnityEngine.RuntimeInitializeOnLoadMethod]
    

    public override void OnStartServer()
    {
        //Ŭ��κ��� ����ó���� ���� �ڵ鷯 ����
        NetworkServer.RegisterHandler<AuthReqMsg>(OnAuthRequestMessage, false);
    }

    public override void OnStopServer()
    {
        //�������� �� ���� ���� �ڵ鷯 ��� ���� 
        NetworkServer.UnregisterHandler<AuthResMsg>();
    }


    public void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthReqMsg msg)
    {
        // Ŭ���̾�Ʈ�κ��� ���� ��û �޽����� �������� �� ȣ��
        Debug.Log($"���� ��û: {msg.authuserName}");

        // �̹� ���� ��� ���� �����̸� ó�� ����
        if (_connectionsPendingDisconnect.Contains(conn)) return;

        // �ܺ� ������ DB�� ȣ���Ͽ� ���� Ȯ�� (������ ������ �÷��̾� �̸� �ߺ� �˻�)
        if (!_playerNames.Contains(msg.authuserName))
        {
            // �ߺ����� ���� ���
            _playerNames.Add(msg.authuserName);

            // ���� ������ ����
            conn.authenticationData = msg.authuserName;

            // ���� ���� �޽��� ���� �� ����
            AuthResMsg authReqMsg = new AuthResMsg
            {
                code = 100,
                msg = "��������"
            };
            conn.Send(authReqMsg);
           ServerAccept(conn);
        }
        else
        {
            // �ߺ��� ���
            _connectionsPendingDisconnect.Add(conn);

            // ���� ���� �޽��� ���� �� ����
            AuthResMsg authResMsg = new AuthResMsg
            {
                code = 200,
                msg = "�̹� ��� ���� �̸��Դϴ�. �ٸ� �̸��� �������ּ���."
            };
            conn.Send(authResMsg);
            conn.isAuthenticated = false;

            // ���� �� ���� ���� ó���ϴ� �ڷ�ƾ
            StartCoroutine(DelayedDisconnect(conn, 1.0f));
        }
    }

    IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float waitTime)
    {
        //���� �ð� ��� �� ���� ����
        yield return new WaitForSeconds(waitTime);
        ServerReject(conn);//�������� ���� ���� ó��

        yield return null;
        _connectionsPendingDisconnect.Remove(conn);//�������� �� ������� ���� ��Ͽ��� ���� 
    }
    #endregion
}