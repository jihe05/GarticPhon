using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Start : NetworkBehaviour
{

    [SerializeField] GameObject PlayerInfoPrefab;//Ŭ���̾�Ʈ ���� ������Ʈ
    [SerializeField] Transform PlayerInfoParent;// // �� �θ�
    

   internal static string _hostPlayerName;

    //���� �¸� - ����� �÷��̾��� �̸�
    internal static readonly Dictionary <NetworkConnectionToClient, string> _connectedNameDic = new Dictionary<NetworkConnectionToClient, string> ();

    //�÷��̾��� �̸� ����
    public void SetLocalPlayername(string playerName)
    { 
       _hostPlayerName = playerName;
    }

    //��������
    public override void OnStartServer()
    { 
        this.gameObject.SetActive (true);
       

    }

    //Ŭ���̾�Ʈ ����
    public override void OnStartClient()
    {
        this.gameObject.SetActive(true);
      
    }




    //Ŭ���̾�Ʈ���� ������ ȣ��(���� ���̵� ����� ���� �� ����)
    [Command(requiresAuthority = false)]
    //sender : ����� ���� Ŭ���̾�Ʈ�� ��������
    void CmdSendName(string name, NetworkConnectionToClient sender = null)
    {
        //Ŭ���̾�Ʈ�� ������ �߰�
        if (!_connectedNameDic.ContainsKey(sender))
        {
            //Ŭ���̾�Ʈ�� �̸��� ��ųʸ��� �߰�
            _connectedNameDic[sender] = name;
        
        }
    
    }

    //UI���� ȣ���Ͽ� �̸��� ������ ����
    public void SendernameToSarver(string playerName)
    {
        if (isClient)
        {
            //������ �̸��� ������ ���
            GarticUser localPlayer = NetworkClient.connection.identity.GetComponent<GarticUser>();
            if (localPlayer != null) 
            {
               localPlayer.name = playerName;
            }
        }


    
    }
}
