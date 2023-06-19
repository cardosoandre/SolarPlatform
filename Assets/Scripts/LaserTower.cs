using System.Collections;
using System.Linq;
using UnityEngine;

public class LaserTower : Defence
{
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Transform laserStart;
    [SerializeField] private int laserResolution;
    [SerializeField] private float laserNoise;
    [SerializeField] private int dmg;

    [SerializeField] private float attackRange;
    [SerializeField] private float timeBetweenAttacks = 3;
    [SerializeField] ParticleSystem shootingRangeEffect;
    

    private float t = 0;

    private void Start()
    {
        shootingRangeEffect.Stop();
    }

    void Update()
    {
        t += Time.deltaTime;
        
        if(t > timeBetweenAttacks)
        {
            t = 0;
            
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, attackRange);
        
            if(nearbyColliders.Length == 0) return;

            foreach (var col in nearbyColliders.Reverse())
            {
                if (col.TryGetComponent(out Enemy enemy))
                {
                    StartCoroutine(Shoot(enemy));
                }
            }
        }
    }

    IEnumerator Shoot(Enemy target)
    {
        Vector3 dir = (target.transform.position - laserStart.position) / laserResolution;      
        lr.positionCount = laserResolution;
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < laserResolution; i++)
        {
            Vector3 offset = Random.insideUnitSphere;
            if (i == 0 || i == laserResolution - 1) offset = Vector3.zero;
            pos = laserStart.position + (dir * i + offset) * laserNoise;

            lr.SetPosition(i, pos);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.05f);

        lr.positionCount = 0;
        Instantiate(hitEffect, target.transform.position, hitEffect.transform.rotation);
        CameraShake.instance.Shake(1);
        target.GetComponent<Health>().TakeDamage(dmg);

    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void SetAsPlaceholder()
    {
        ParticleSystem.ShapeModule shapeRadius = shootingRangeEffect.shape;
        shapeRadius.radius = attackRange;
        this.enabled = false;
    }
}