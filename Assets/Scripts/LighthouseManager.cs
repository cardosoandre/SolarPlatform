using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighthouseManager : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionEffect;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(int dmg)
    {
        GameManager.instance.UpdateHealth(-dmg);

        if (GameManager.instance.currHealth <= 0)
        {
            explosionEffect.Play();
        }
    }
}
