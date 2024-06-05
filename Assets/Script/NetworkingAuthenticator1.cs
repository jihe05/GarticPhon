using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkingAuthenticator
{
    [SerializeField] Login _login;

    [Header("Client Username")]
    public string _playerName;

    public void OnInputValueChaged_SetPlayerName(string userName)
    { 
      _playerName = userName;
        _login.SetUIChanged();
    }

    public override void OnStartClient()
    {
        NetworkClient.RegisterHandler<AuthResMsg>(OnAuthResponseMessage, false);
    }

    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<AuthReqMsg>();
    }

    public override void OnClientAuthenticate()
    {
        NetworkClient.Send(new AuthReqMsg { authuserName = _playerName });
    }

    public void OnAuthResponseMessage(AuthResMsg msg)
    {
        if (msg.code == 100)
        { 
          Debug.Log($"º“»Ø :{msg.code}{msg.mess}")
        }
    }

}
