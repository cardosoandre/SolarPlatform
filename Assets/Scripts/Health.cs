using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public UnityEvent onDeathEvent;
    public GameObject healthBarPrefab;
    public Vector3 barOffset;

    private Image healthBar;
    private float startHealth;
    public void Start()
    {
        GameObject bar = Instantiate(healthBarPrefab, transform.position + barOffset, healthBarPrefab.transform.rotation);
        bar.transform.parent = transform;
        healthBar = bar.transform.Find("BarFill").GetComponent<Image>();
        startHealth = health;
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        healthBar.fillAmount = health / startHealth;
        if (health <= 0) onDeathEvent.Invoke();
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
