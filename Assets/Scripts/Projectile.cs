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
            CollideWithTarget(.4f);
        }
        else if(collision.transform.tag == "Lighthouse")
        {
            collision.transform.GetComponent<LighthouseManager>().TakeDamage(dmgToLighthouse);
            CollideWithTarget(1);
        }
    }

    void CollideWithTarget(float size)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
        effect.transform.localScale = new Vector3(size, size, size);
        CameraShake.instance.Shake(size);
        Destroy(gameObject);
    }

}