using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    public float movementSpeed;
    public int damage;
    public GameObject hitEffect;

    void Start()
    {
        Destroy(gameObject, 10f);
    }
    void Update()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }
    
    public void SetAsPlaceholder()
    {
        this.enabled = false;
    }

    public void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Enemy")
        {
            Instantiate(hitEffect, transform.position - transform.forward * 2, hitEffect.transform.rotation);
            other.GetComponent<Health>().TakeDamage(damage);
            CameraShake.instance.Shake(1);
        }
    }
}
