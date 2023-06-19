using System;
using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int dmg;
    public int dmgToLighthouse = 5;
    public GameObject hitEffect;
    private Vector3 direction;
    private float speed;

    void Awake()
    {
        Vector3 startScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(startScale, 0.2f).SetEase(Ease.OutExpo);
    }
    
    public void Init(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }

    void Update()
    {
        if(speed == 0) return;
        
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Build")
        {
            collision.transform.GetComponent<Health>().TakeDamage(dmg);
            CollideWithTarget();
        }
        else if(collision.transform.tag == "Lighthouse")
        {
            collision.transform.GetComponent<LighthouseManager>().TakeDamage(dmgToLighthouse);
            CollideWithTarget();
        }
    }

    void CollideWithTarget()
    {
        Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
        CameraShake.instance.Shake(1);
        Destroy(gameObject);
    }

}