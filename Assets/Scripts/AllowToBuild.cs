using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowToBuild : MonoBehaviour
{
    public GameObject takenPlatform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (takenPlatform != null)
        {
            takenPlatform.gameObject.layer = 8;
            takenPlatform.gameObject.tag = "Platform";
        }
    }
}
