using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject GameCam;
    [SerializeField] GameObject PauseCam;

    public static PauseMenu getInstance() => GameObject.FindWithTag("PauseMenu")?.GetComponent<PauseMenu>();


    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Region").GetComponent<TextMeshProUGUI>().SetText($"{PhotonNetwork.CloudRegion}");
        GameObject.Find("Ping").GetComponent<TextMeshProUGUI>().SetText($"{PhotonNetwork.GetPing()} ms");
    }

    public void Resume(){
        PauseCam.SetActive(false);
        GameObject.Find("PauseMenu").SetActive(false);
        GameCam.SetActive(true);
    }

    public void HowToRedirect(){
        Application.OpenURL("https://github.com/NotAddison/GPN_ASG2#how-to-play");
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
        
        // Trigger OST Again
        PreserveSound.Instance.gameObject.GetComponent<AudioSource>().time = 0;
        PreserveSound.Instance.gameObject.GetComponent<AudioSource>().Play();
    }
}
