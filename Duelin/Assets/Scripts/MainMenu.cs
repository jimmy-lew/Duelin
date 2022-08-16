using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class MainMenu : MonoBehaviour
{
    void Awake(){
        if (GameObject.Find("BuildVer") != null)
        {
            GameObject.Find("BuildVer").GetComponent<TextMeshProUGUI>().SetText($"{Application.version}");
        }

        if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
    }

    public void Play(){
        SceneManager.LoadScene("Connecting");
    }

    public void Settings(){
        SceneManager.LoadScene("Settings");
    }

    public void HowToRedirect(){
        Application.OpenURL("https://github.com/NotAddison/GPN_ASG2#how-to-play");
    }

    public void Credits(){
        SceneManager.LoadScene("Credits");
    }

    public void Quit(){
        Application.Quit();
    }

    public void ReturnToMain(){
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }
}
