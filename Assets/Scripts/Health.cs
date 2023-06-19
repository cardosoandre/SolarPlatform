using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class Health : MonoBehaviour
{
    public int health;
    public UnityEvent onDeathEvent;
    public GameObject healthBarPrefab;
    public GameObject smokePrefab;
    public GameObject shockPrefab;

    [Header("Hit Blink")]
    public Renderer renderer;
    [SerializeField] [ColorUsage(true, true)] Color normalEmissionColor;
    [SerializeField] [ColorUsage(true, true)] Color hitEmissionColor;

    [Space]
    public Vector3 barOffset;
    bool firstHealthWarning;
    bool secondHealthWarning;

    private Image healthBar;
    private float startHealth;
    public void Start()
    {
        //GameObject bar = Instantiate(healthBarPrefab, transform.position + barOffset, healthBarPrefab.transform.rotation);
        //bar.transform.parent = transform;
        //healthBar = bar.transform.Find("BarFill").GetComponent<Image>();
        startHealth = health;
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if(renderer != null)
        renderer.material.DOColor(hitEmissionColor, "_EmissionColor", .1f).OnComplete(() => renderer.material.DOColor(normalEmissionColor, "_EmissionColor", .1f));


        float healthPercentage = health / 100f;


        if (healthPercentage < 0.4f && !firstHealthWarning)
        {
            firstHealthWarning = true;
            GameObject smoke = Instantiate(smokePrefab, transform.position, transform.rotation, transform);

        }

        if (healthPercentage < 0.2f && !secondHealthWarning)
        {
            secondHealthWarning = true;
            GameObject shock = Instantiate(shockPrefab, transform.position, transform.rotation, transform);
        }

        //healthBar.fillAmount = health / startHealth;
        if (health <= 0) onDeathEvent.Invoke();
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
