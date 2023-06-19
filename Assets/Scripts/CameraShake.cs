using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount, shakeDuration;
    public static CameraShake instance { get; private set; }

    private Transform cam;
    private float curShakeAmount, curShakeDuration;
    private Vector3 orgPos;

    private void Start()
    {
        instance = this;
        cam = FindObjectOfType<Camera>().transform;
        this.enabled = false;
    }

    private void Update()
    {
        if (curShakeDuration > 0)
        {
            cam.position = orgPos + Random.insideUnitSphere * curShakeAmount;
            curShakeDuration -= Time.deltaTime;
        }
        else
        {
            cam.position = orgPos;
            this.enabled = false;
        }
    }

    public void Shake(float multiplier)
    {
        orgPos = cam.position;
        curShakeAmount = shakeAmount * multiplier;
        curShakeDuration = shakeDuration * multiplier;
        this.enabled = true;
    }
}
