using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminateSound : MonoBehaviour
{
    // Terminates OST that is being "preserved"
    void Start(){
        PreserveSound.Instance.gameObject.GetComponent<AudioSource>().Pause();
    }
}
