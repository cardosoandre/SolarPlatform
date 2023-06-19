using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LighthouseManager : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] Renderer renderer;
    [SerializeField] [ColorUsage(true,true)] Color normalEmissionColor;
    [SerializeField] [ColorUsage(true, true)] Color hitEmissionColor;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(int dmg)
    {
        GameManager.instance.UpdateHealth(-dmg);

        renderer.material.DOColor(hitEmissionColor, "_EmissionColor", .05f).OnComplete(() => renderer.material.DOColor(normalEmissionColor, "_EmissionColor", .1f));

        if (GameManager.instance.currHealth <= 0)
        {
            explosionEffect.Play();
        }
    }
}
