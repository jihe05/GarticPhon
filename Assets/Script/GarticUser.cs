using System.Collections;
using UnityEngine;
using Mirror;

//������ Ŭ���̾�Ʈ���� ����ȭ�ϰ� , �÷��̾� �̸��� UI�� ���� 
public class GarticUser : NetworkBehaviour
{
    // SyncVar - ���� ������ ��� Ŭ�� �ڵ� ����ȭ�ϴµ� ����
    // Ŭ�� ���� �����ϸ� �ȵǰ�, �������� �����ؾ� ��
    [SyncVar]
    public string PlayerName;

    //�������� �÷��̾ ����ɶ� ȣ�� �Ǵ� �Լ�
    public override void OnStartServer()
    {
        //�������� �÷��̾ ����ɶ� Playername�� ���� �����ͷ� ���
        PlayerName = (string)connectionToClient.authenticationData;
    }

    //���� �޼���� ���� ���������� ȣ��
    public override void OnStartLocalPlayer()
    {
        //Startpanel������Ʈ�� ã�� �÷��̾��� �̸��� UI�� ����
        var objStartUI = GameObject.Find("Start Panel");

        if (objStartUI != null)
        {
            //Start��ũ��Ʈ�� ������Ʈ�� ������ 
            var StartPanel = objStartUI.GetComponent<waitpanel>();
            if (StartPanel != null)
            {
                //�÷��̾��� �̸��� UI�� ����
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
