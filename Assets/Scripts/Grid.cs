using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour{

    [SerializeField] private LineRenderer renderer;

    [SerializeField] private int size;
    [SerializeField] private float height;

    [SerializeField] private float tileSize;
    [SerializeField] private Vector3 offset;

    private void Start(){
        renderer.positionCount = size * 2;
        
        Vector3 startPos = new Vector3(-size / 2f * tileSize, height, -size / 2f * tileSize);
        startPos += offset;
        int direction = 1;
        
        int current = 0;

        for(int x = 0; x < size; x++){
            Vector3 pos1 = startPos + Vector3.right * x * tileSize;
            renderer.SetPosition(current, pos1);
            
            Vector3 pos2 = pos1 + Vector3.forward * size * direction * tileSize;
            renderer.SetPosition(current + 1, pos2);

            current += 2;
            direction *= -1;

            Vector3 newStartPos = startPos;
            newStartPos.z *= -1;
            startPos = newStartPos;
        }
    }
}
