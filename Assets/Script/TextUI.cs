using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;


public class TextUI : NetworkBehaviour
{

    public Image fillImage;
    public float fillcount = 30;

    public InputField inputField;

    public GameObject drowingObj;

    // 버튼을 클릭한 플레이어를 추적하는 해시셋
    private static HashSet<NetworkConnectionToClient> playersWhoClickedButton = new HashSet<NetworkConnectionToClient>();
    private static int totalPlayers;

    void OnEnable()
    {
        NetworkManager.singleton.OnServerAddPlayer += OnServerAddPlayer;
        NetworkManager.singleton.OnServerDisconnect += OnServerDisconnect;


    }

    void OnDisable()
    {
        NetworkManager.singleton.OnServerAddPlayer -= OnServerAddPlayer;
        NetworkManager.singleton.OnServerDisconnect -= OnServerDisconnect;
    }

    void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        totalPlayers = NetworkServer.connections.Count;
        Debug.Log("Player connected. Total players: " + totalPlayers);
    }

    void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        totalPlayers = NetworkServer.connections.Count;
        Debug.Log("Player disconnected. Total players: " + totalPlayers);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        totalPlayers = NetworkServer.connections.Count; // 서버에서 총 플레이어 수 설정
      
    }

    public void Start()
    {

        StartCoroutine(TimeCount());

    }


    //타이머
    IEnumerator TimeCount()
    {
        float elapsedTime = 0f;
        float startFillAmount = fillImage.fillAmount;

        while (elapsedTime < fillcount)
        {
            elapsedTime += Time.deltaTime;
            fillImage.fillAmount = Mathf.Lerp(startFillAmount, 0f, elapsedTime / fillcount);

            yield return null;
        }


        fillImage.fillAmount = 0f;
        // 타이머가 0초가 되었을 때 RPC 호출
        CmdTimerEnded();

    }

    public void OnSaveButtonClick()
    {
        Debug.Log("Total players: " + totalPlayers);
        SaveInput(); // 입력 내용을 저장
        CmdPlayerClickedButton(); // 서버로 명령 전송
    }

    // 서버로 타이머 종료 명령 전송 (서버에서 실행됨)
    [Command(requiresAuthority = false)]
    private void CmdTimerEnded(NetworkConnectionToClient sender = null)
    {
        RpcDisableObject(); // 모든 클라이언트에 오브젝트 비활성화 호출
    }

    // InputField 내용을 PlayerPrefs에 저장
    private void SaveInput()
    {
        string inputValue = inputField.text; // 입력 필드의 내용을 가져옴
        PlayerPrefs.SetString("SavedInput", inputValue); // PlayerPrefs에 값 저장
        PlayerPrefs.Save(); // 저장된 값을 디스크에 기록
        Debug.Log("Input saved: " + inputValue);
    }

    // 서버로 명령 전송 (명령은 서버에서 실행됨)
    [Command(requiresAuthority = false)]
    private void CmdPlayerClickedButton(NetworkConnectionToClient sender = null)
    {

        Debug.Log("플레이어 수 확인");
        if (sender != null)
        {
            if (!playersWhoClickedButton.Contains(sender))
            {
                playersWhoClickedButton.Add(sender); // 버튼을 클릭한 플레이어 추가
            }

            // 모든 플레이어가 버튼을 클릭했는지 확인
            if (playersWhoClickedButton.Count >= totalPlayers)
            {
                Debug.Log("모든 플레이어가 버튼을 클릭함");
                RpcDisableObject(); // 모든 클라이언트에 오브젝트 비활성화 호출
            }
        }
    }

    // 클라이언트에서 특정 오브젝트 비활성화
    [ClientRpc]
    private void RpcDisableObject()
    {
        if (drowingObj != null)
        {
            drowingObj.SetActive(false); // 오브젝트 비활성화
            Debug.Log("Target object disabled");
        }
        else
        {
            Debug.LogWarning("null");
        }
    }

}
