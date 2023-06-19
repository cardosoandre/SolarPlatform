using System;
using UnityEngine;

public class SimpleRotate : MonoBehaviour{

    [SerializeField] private Vector3 rotation;

    private void Update(){
        transform.Rotate(rotation * Time.deltaTime, Space.World);
    }
}
