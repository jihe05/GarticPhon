using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] internal InputField InputNetwork;//네트워크 주소
    [SerializeField] internal InputField InputuserName;//사용자 이름

    [SerializeField] internal Button Button_Login;
    [SerializeField] internal Button Button_Client;

    [SerializeField] internal Text Text_Error;
 

    [SerializeField] NetworkManager netmanager;

    public GameObject falseObjText;
    public GameObject falseObjDrow;
    public static Login Instance { get; private set; }

    private string originNetAddre;


    private void Awake()
    {
        Instance = this;
        Text_Error.gameObject.SetActive(false);
    }

    private void Start()
    {
        SetDefaultnetwork();
        //기본 네트워크 주소 설정
    }

    private void OnEnable()
    {
        //사용자 이름 변경 리스너 추가
        InputuserName.onValueChanged.AddListener(Changed_ToggleButton);
    }

    private void OnDisable()
    {
        //사용자 이름 변경 리스너 제거
        InputuserName.onValueChanged.RemoveListener(Changed_ToggleButton);
    }

    private void Update()
    {
        //네트워크 주소 유효성 검사
        CheckNetwork();
    }

    private void SetDefaultnetwork()
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = "GarticPhone";
        
        }

        //비어있으면 원래 주소로 설정
        originNetAddre = NetworkManager.singleton.networkAddress;
    
    
    }

    private void CheckNetwork()
    {
        //네트워크 주소가 비어있으면 원래 주소로 설정
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = originNetAddre;
        
        }

        //입력필드와 네트워크 주소가 다르면 입력 필드 업데이트
        if (InputNetwork.text != NetworkManager.singleton.networkAddress)
        { 
           InputNetwork.text = NetworkManager.singleton.networkAddress;
        
        }
    
    
    }

    //클라이언트가 끊겼을때
    public void SetUIclient()
    { 
        this.gameObject.SetActive(true);
        InputuserName.text = string.Empty;
        //인풋필드 활성화
        InputuserName.ActivateInputField();


    }

    //인증값이 변경되었을때 UI설정
    public void SetUIChanged()
    { 
       Text_Error.text = string.Empty;
       Text_Error.gameObject.SetActive(false);
    
    }

    //사용자이름이 변경되었을때
    public void SetUIError(string msg)
    {
        Text_Error.text = msg;
        Text_Error.gameObject.SetActive(true);

    }

    public void Changed_ToggleButton(string username)
    { 
      //유효한지 확인하고 False와 True값을 반환
       bool isUserName = !string.IsNullOrWhiteSpace(username);
       Button_Login.interactable = isUserName;
       Button_Client.interactable = isUserName;
    
    }

    public void OnClick_StartHost()
    {
        if (netmanager == null)
            return;

        netmanager.StartHost();
        this.gameObject.SetActive(false);
        falseObjText.SetActive(false);
        falseObjDrow.SetActive(false);
    }
    
    public void OnClick_StartClient()
    {
        if (netmanager == null)
            return;

        netmanager.StartClient();
        this.gameObject.SetActive(false);
        falseObjText.SetActive(false);
        falseObjDrow.SetActive(false);


    }
}
