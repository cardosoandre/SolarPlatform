using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected bool isDead = false;
    public enum EnemyType
    {
        basic = 1,
        speed = 2,
        split = 3
    }

    public EnemyType enemyType = EnemyType.basic;
    [SerializeField] ParticleSystem DrownEffect;

    [Header("Split Enemy Type attributes")]
    [SerializeField] GameObject spawnedEnemies;
    [SerializeField] int spawnCount = 3;

    public IEnumerator Die()
    {
        isDead = true;
        yield return new WaitForSeconds(0.5f);
        Instantiate(DrownEffect.gameObject, transform.position, Quaternion.identity);
        transform.DOMoveY(transform.position.y - 2, 1f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(1);
        
        if(enemyType == EnemyType.split)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Instantiate(spawnedEnemies, new Vector3((Random.Range(transform.localPosition.x - 3, transform.localPosition.x + 3) ), spawnedEnemies.transform.position.y, transform.localPosition.z), spawnedEnemies.transform.rotation);
            }
        }

        Destroy(gameObject);
    }
}