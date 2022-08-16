using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker getInstance() => GameObject.FindWithTag("MainCamera").GetComponent<CameraShaker>();
    public Transform cam;
    public float power = 0.03f;
    public float duration = 0.05f;
    public float slowDownAmount = 1f;
    public bool toggle = false;

    Vector3 startPosition;
    float startDuration;

    void Start()
    {
        cam = Camera.main.transform;
        startPosition = cam.localPosition;
        startDuration = duration;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(toggle){
            if(duration > 0){
                cam.localPosition = cam.localPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }else{
                toggle = false;
                duration = startDuration;
                cam.localPosition = startPosition;
            }
        }
    }
}
