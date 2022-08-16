using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI Status;
    void Start()
    {
        Status.text = ("Connecting...");
        Debug.Log("Connecting to Server");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Status.SetText("Connected!");
        Debug.LogError("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Status.SetText("Joining Lobby...");
        SceneManager.LoadScene("LobbyMenu");
    }
}
