using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public float OSTVol = 1f;
    public float SFXVol = 1f;
    public float AmbienceVol = 1f;

    public static SettingsMenu getInstance() => GameObject.FindWithTag("Settings")?.GetComponent<SettingsMenu>();

    void Awake(){
        // Singleton
        if (GameObject.FindGameObjectsWithTag("Settings").Length > 1){
            Destroy(gameObject);
        }
    }

    void Start(){
        Debug.LogError("Player Prefs null: " + (PlayerPrefs.HasKey("OSTVol")));

        if (PlayerPrefs.HasKey("OSTVol")){GameObject.Find("Music").GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("OSTVol");}
        else GameObject.Find("Music").GetComponent<UnityEngine.UI.Slider>().value = PreserveSound.Instance.gameObject.GetComponent<AudioSource>().volume;

        if (PlayerPrefs.HasKey("SFXVol")){GameObject.Find("SFX").GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("SFXVol");}
        else GameObject.Find("SFX").GetComponent<UnityEngine.UI.Slider>().value = 1f;

        if (PlayerPrefs.HasKey("AmbienceVol")){GameObject.Find("Ambience").GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("AmbienceVol");}
        else GameObject.Find("Ambience").GetComponent<UnityEngine.UI.Slider>().value = 1f;
    }

    public void SetOSTVol(float vol){
        PreserveSound.Instance.gameObject.GetComponent<AudioSource>().volume = vol;
        Debug.LogError($"[Settings] Volume set to {vol}");
        OSTVol = vol;
        PlayerPrefs.SetFloat("OSTVol", vol);
    }

    public float GetOSTVol(){
        return OSTVol;
    }

    public void SetSFXVol(float vol){
        Debug.LogError($"[Settings] SFX Volume set to {vol}");
        SFXVol = vol;
        PlayerPrefs.SetFloat("SFXVol", vol);
    }

    public float GetSFXVol(){
        return SFXVol;
    }

    public void SetAmbienceVol(float vol){
        Debug.LogError($"[Settings] Ambience Volume set to {vol}");
        AmbienceVol = vol;
        PlayerPrefs.SetFloat("AmbienceVol", vol);
    }

    public float GetAmbienceVol(){
        return AmbienceVol;
    }

    public void ReturnToMain(){
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
