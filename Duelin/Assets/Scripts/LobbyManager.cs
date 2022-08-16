using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Lobby")]
    [SerializeField] GameObject createInput;
    [SerializeField] GameObject joinInput;
    [SerializeField] GameObject LobbyLog;
    [SerializeField] private byte maxPlayers = 4;

    [Header("Room Menu")]
    [SerializeField] GameObject RoomMenu;
    [SerializeField] GameObject RoomMenuLog;
    [SerializeField] GameObject RoomCode;
    [SerializeField] GameObject PlayerNum;
    [SerializeField] GameObject LocalPlayerNo;

    void Start(){
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        LobbyLog.SetActive(false);
        RoomMenu.SetActive(false);
    }

    void Awake(){
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        string code = createInput.GetComponent<TMP_InputField>().text;
        if(code == ""){
            LobbyLog.GetComponent<TextMeshProUGUI>().text = "Please enter a room code";
            LobbyLog.SetActive(true);
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(code, roomOptions);
        Debug.LogError($"[Photon] Creating Room: {code} | Max Players: {roomOptions.MaxPlayers} | Server: {PhotonNetwork.CloudRegion}");

        // UI Display
        string text = RoomMenuLog.GetComponent<TextMeshProUGUI>().text;
        RoomMenuLog.GetComponent<TextMeshProUGUI>().SetText(text + $"[Photon] Server: {PhotonNetwork.CloudRegion}");
        RoomMenuLog.GetComponent<TextMeshProUGUI>().SetText(text + $"\n[Photon] Ping: {PhotonNetwork.GetPing()} ms");
        RoomCode.GetComponent<TextMeshProUGUI>().SetText(code);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        LobbyLog.SetActive(true);
        LobbyLog.GetComponent<TextMeshProUGUI>().SetText($"[Photon] Failed to create room: {message}");
        Debug.LogError($"[Photon]: Room creation failed: {message}");
    }

    public void JoinRoom()
    {
        string code = joinInput.GetComponent<TMP_InputField>().text;
        // Debug.LogError($"[Photon] Joining Room: {code} | Server: {PhotonNetwork.CloudRegion}");
        Debug.LogError($"[Photon] AutomaticallySyncScene Status: {PhotonNetwork.AutomaticallySyncScene}");
        PhotonNetwork.JoinRoom(code);
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError($"[Photon] Joined Room: {PhotonNetwork.CurrentRoom.Name} | Server: {PhotonNetwork.CloudRegion}");
        Debug.LogError($"[Photon] Players in Room: {PhotonNetwork.CurrentRoom.PlayerCount}");
        RoomMenu.SetActive(true);

        // UI Display
        PlayerNum.GetComponent<TextMeshProUGUI>().SetText($"{PhotonNetwork.CurrentRoom.PlayerCount} | {PhotonNetwork.CurrentRoom.MaxPlayers}");
        LocalPlayerNo.GetComponent<TextMeshProUGUI>().SetText($"Player {PhotonNetwork.LocalPlayer.ActorNumber}");
        RoomCode.GetComponent<TextMeshProUGUI>().SetText(PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"[Photon]: Room join failed: {message}");

        // UI Error Message
        LobbyLog.SetActive(true);
        LobbyLog.GetComponent<TextMeshProUGUI>().SetText($"[Photon] Failed to create room: {message}");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        RoomMenu.SetActive(false);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        string text = RoomMenuLog.GetComponent<TextMeshProUGUI>().text;
        RoomMenuLog.GetComponent<TextMeshProUGUI>().SetText(text + $"\n[Photon] Player {otherPlayer.ActorNumber} has left the room");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogError("[Photon]: Player entered room: " + newPlayer.ActorNumber);

        // UI Display
        string text = RoomMenuLog.GetComponent<TextMeshProUGUI>().text;
        RoomMenuLog.GetComponent<TextMeshProUGUI>().SetText(text + $"\n[Photon] Player {newPlayer.ActorNumber} has joined the room");
        PlayerNum.GetComponent<TextMeshProUGUI>().SetText($"{PhotonNetwork.CurrentRoom.PlayerCount} | {PhotonNetwork.CurrentRoom.MaxPlayers}");
    }

    public void StartGame(){
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1){ PhotonNetwork.LoadLevel("Game"); PhotonNetwork.CurrentRoom.IsOpen = false;} 
            else 
            {
                Debug.LogError("[Photon]: Not enough players!"); 
                string text = RoomMenuLog.GetComponent<TextMeshProUGUI>().text;
                RoomMenuLog.GetComponent<TextMeshProUGUI>().SetText(text + $"\n[Photon]: Not enough players!");
            }
        }
        else
        {
            Debug.LogError("[Photon]: Only the Host Client can start the game");
            string text = RoomMenuLog.GetComponent<TextMeshProUGUI>().text;
            RoomMenuLog.GetComponent<TextMeshProUGUI>().SetText(text + "\n[Photon]: Only the Host Client can start the game");
        }
    }
}
