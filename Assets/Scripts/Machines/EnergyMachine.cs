using System;
using UnityEngine;

public class EnergyMachine : MonoBehaviour{

    [SerializeField] private float energyInterval;
    [SerializeField] private int energyAmount;

    [SerializeField] private EnergyEffect effect;

    private GameManager gameManager;

    private float timer;

    private void Start(){
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update(){
        if (GameManager.instance.isGameStarted)
        {
            timer += Time.deltaTime;

            if (timer >= energyInterval)
            {
                AddEnergy();

                timer = 0;
            }
        }
    }

    private void AddEnergy(){
        gameManager.UpdateEnergy(energyAmount);
        
        effect.gameObject.SetActive(true);
        effect.SetText("+" + energyAmount);
    }

    public void SetAsPlaceholder()
    {
        SimpleRotate simpleRotate = GetComponentInChildren<SimpleRotate>();
        if (simpleRotate != null)
            simpleRotate.enabled = false;
        this.enabled = false;
    }
}
