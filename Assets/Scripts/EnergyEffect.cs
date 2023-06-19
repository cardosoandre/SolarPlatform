using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnergyEffect : MonoBehaviour{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float scaleSpeed;

    [SerializeField] private TextMesh text;

    private Vector3 startPos;
    private Vector3 startScale;

    private void Start(){
        startPos = transform.localPosition;
        startScale = transform.localScale;
    }

    private void Update(){
        if(text.text.Equals(""))
            return;
        
        transform.position += Vector3.up * Time.deltaTime * moveSpeed;
        
        transform.localScale -= Vector3.one * Time.deltaTime * scaleSpeed;
        
        if(transform.localScale.x < 0.1f)
            gameObject.SetActive(false);
    }

    public void SetText(string t){
        text.text = t;
        transform.localPosition = startPos;
        transform.localScale = startScale;
    }
}
