using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreserveSound : MonoBehaviour
{
    [SerializeField] AudioSource clickSource;

    // This script basically preserves the sound so that the OST continues playing until it is called to stop
    private static PreserveSound instance = null;
    public static PreserveSound Instance
    {
        get { return instance; }
    }

    void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject); // Prevents obj from being destroyed when changing scenes
    }


    void Update(){
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            if (!PlayerPrefs.HasKey("SFXVol")) clickSource.PlayOneShot(clickSource.clip, 1f);
            else clickSource.PlayOneShot(clickSource.clip, PlayerPrefs.GetFloat("SFXVol"));
        }
    }


}
