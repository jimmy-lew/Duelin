using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject GameCam;
    [SerializeField] GameObject PauseCam;
    [SerializeField] GameObject PauseMenu;

    public static GameManager getInstance() => GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    List<Vector3> spawnPositions = new List<Vector3>(){
        new Vector3(     0, -0.48f, 0),
        new Vector3(     0,  0.80f, 0),
        new Vector3(-1.28f,  0.16f, 0),
        new Vector3( 1.28f,  0.16f, 0),
    };

    public int amountToWin = 25;

    void Start()
    {
        AudioPlayer("OSTVol","OST 2");
        AudioPlayer("AmbienceVol","Ambience");

        // amountToWin += PhotonNetwork.CountOfPlayersInRooms > 2 ? (PhotonNetwork.CountOfPlayersInRooms - 2) * 5 : 0;
        GameObject.FindWithTag("GoldBar").GetComponent<GoldBar>().RenderBar();
        PhotonNetwork.Instantiate("Prefabs/Structures/spawn", spawnPositions[PhotonNetwork.LocalPlayer.ActorNumber-1], Quaternion.identity);
        TurnManager.getInstance().StartTurn();
    }

    void OnApplicationQuit()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer){
        Debug.LogError($"[GameManager]: Player {otherPlayer.ActorNumber} has left the room");

        // Check if last one = Win by elimination
        Debug.LogError("NoPlayers = " + PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.LogError("Win Eliminated?" + (PhotonNetwork.CurrentRoom.PlayerCount == 1));
        if (PhotonNetwork.CountOfPlayersInRooms == 1){
            GameObject.FindWithTag("WinLoseToast").GetComponent<WinLoseToast>().Render(true);
        } 
    }

    [PunRPC]
    public void AddGameState(int stateInt, int duration)
    {
        LocalInventory.getInstance().AddGameState((LocalInventory.GAME_STATE) stateInt, duration);
    }

    public void UpdateGameState(LocalInventory.GAME_STATE state, int duration)
    {
        PhotonView.Get(this).RPC("AddGameState", RpcTarget.All, (int) state, duration);
    }

    public void AudioPlayer(string key,string name, float vol = 1f)
    {
        if (!PlayerPrefs.HasKey(key)) FindObjectOfType<AudioManager>().Play(name, vol);
        else FindObjectOfType<AudioManager>().Play(name, PlayerPrefs.GetFloat(key));
    }

    public void HandleWin()
    {
        Win();
        PhotonView.Get(this).RPC("Lose", RpcTarget.Others);
    }

    private void Win()
    {
        GameObject.FindWithTag("WinLoseToast").GetComponent<WinLoseToast>().Render();
    }

    [PunRPC]
    private void Lose()
    {
        GameObject.FindWithTag("WinLoseToast").GetComponent<WinLoseToast>().Render(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
            PauseCam.SetActive(true);
            GameCam.SetActive(false);
        }
    }
}