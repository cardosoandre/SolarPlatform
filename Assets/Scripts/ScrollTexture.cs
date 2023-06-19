using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this would be better using a shader but I can't use amplify
public class ScrollTexture : MonoBehaviour{

    [SerializeField] private Material target;
    [SerializeField] private Vector2 scrollAmount;

    private void Start(){
        target.mainTextureOffset = Vector2.zero;
    }

    private void Update(){
        target.mainTextureOffset += scrollAmount * Time.deltaTime;
    }
}
