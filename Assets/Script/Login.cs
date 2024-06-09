using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] internal InputField InputNetwork;//��Ʈ��ũ �ּ�
    [SerializeField] internal InputField InputuserName;//����� �̸�

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
        //�⺻ ��Ʈ��ũ �ּ� ����
    }

    private void OnEnable()
    {
        //����� �̸� ���� ������ �߰�
        InputuserName.onValueChanged.AddListener(Changed_ToggleButton);
    }

    private void OnDisable()
    {
        //����� �̸� ���� ������ ����
        InputuserName.onValueChanged.RemoveListener(Changed_ToggleButton);
    }

    private void Update()
    {
        //��Ʈ��ũ �ּ� ��ȿ�� �˻�
        CheckNetwork();
    }

    private void SetDefaultnetwork()
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = "GarticPhone";
        
        }

        //��������� ���� �ּҷ� ����
        originNetAddre = NetworkManager.singleton.networkAddress;
    
    
    }

    private void CheckNetwork()
    {
        //��Ʈ��ũ �ּҰ� ��������� ���� �ּҷ� ����
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = originNetAddre;
        
        }

        //�Է��ʵ�� ��Ʈ��ũ �ּҰ� �ٸ��� �Է� �ʵ� ������Ʈ
        if (InputNetwork.text != NetworkManager.singleton.networkAddress)
        { 
           InputNetwork.text = NetworkManager.singleton.networkAddress;
        
        }
    
    
    }

    //Ŭ���̾�Ʈ�� ��������
    public void SetUIclient()
    { 
        this.gameObject.SetActive(true);
        InputuserName.text = string.Empty;
        //��ǲ�ʵ� Ȱ��ȭ
        InputuserName.ActivateInputField();


    }

    //�������� ����Ǿ����� UI����
    public void SetUIChanged()
    { 
       Text_Error.text = string.Empty;
       Text_Error.gameObject.SetActive(false);
    
    }

    //������̸��� ����Ǿ�����
    public void SetUIError(string msg)
    {
        Text_Error.text = msg;
        Text_Error.gameObject.SetActive(true);

    }

    public void Changed_ToggleButton(string username)
    { 
      //��ȿ���� Ȯ���ϰ� False�� True���� ��ȯ
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
