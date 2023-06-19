using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Boat : Enemy
{
    [Header("General enemy attributes")]
    [SerializeField] private GameObject exploEffect;
    [SerializeField] private float rotTime, reloadTime;
    [SerializeField] private float speed;
    [SerializeField] private float wobbleIntensity;
    [SerializeField] private float attackRange;
    [SerializeField] private Projectile cannonBall;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform barrel;


    private float t;
    private float randomOffset;
    private bool isAttacking;
    private Quaternion goalRot;
    private Transform attackTarget;


    void Start()
    {
        randomOffset = Random.Range(0, Mathf.PI * 2);
    }

    void Update()
    {
        if (GameManager.instance.isLighthouseDestroyed) return;
        if(isDead) return;
        t += Time.deltaTime;
        
        if (!isAttacking)
        {
            if (TryShootSphere()) return;

            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            float xRot = Mathf.Sin(t * Mathf.PI) * wobbleIntensity;
            float zRot = Mathf.Sin((t + randomOffset) * Mathf.PI) * wobbleIntensity;
        
            Quaternion newRot = Quaternion.Euler(xRot, transform.eulerAngles.y, zRot);

            transform.rotation = newRot;           
        }
        else if(attackTarget != null)
        {

            if (transform.rotation != goalRot)
            {
                if (t < rotTime) transform.rotation = Quaternion.Lerp(Quaternion.identity, goalRot, t / rotTime);
                else transform.rotation = goalRot;
            }
            else if(t > reloadTime)
            {
                GameObject effect = Instantiate(exploEffect, barrel.position, exploEffect.transform.rotation);
                effect.transform.localScale *= 0.4f;

                Projectile proj = Instantiate(cannonBall, barrel.position, Random.rotation);
                proj.Init((attackTarget.transform.position - transform.position).normalized, projectileSpeed);
                isAttacking = false;
                goalRot = transform.rotation;
                t = 0;
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(goalRot, Quaternion.identity, t / rotTime);
            if(t >= rotTime) isAttacking = false;
        }

    }

    public void SinkBoat()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(Die());
    }

    public bool TryShootSphere()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, attackRange);
        
        if(nearbyColliders.Length == 0) return false;

        foreach (var col in nearbyColliders.Reverse())
        {
            if (col.tag == "Build")
            {
                Attack(col.transform);
                return true;
            }
            else if (col.tag == "Lighthouse")
            {
                Attack(col.transform);
                return true;
            }
        }

        return false;
    }

    void Attack(Transform energyMachine)
    {
        isAttacking = true;
        attackTarget = energyMachine.transform;
        t = 0;
        Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
        goalRot = Quaternion.FromToRotation(transform.forward, direction) * transform.rotation;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    
    
}
