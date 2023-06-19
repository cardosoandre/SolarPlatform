using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{

    [HideInInspector] public static GameManager instance;
    [HideInInspector] public int currentEnergy;
    public int startingHealth = 100;
    [HideInInspector] public int currHealth;
    [HideInInspector] public bool isLighthouseDestroyed = false;

    [SerializeField] private Text energyText;
    [SerializeField] private float labelEffect;
    [SerializeField] Text healthText;
    [SerializeField] GameObject inGameScreen;
    [SerializeField] GameObject loseScreen;

    [SerializeField] Text FastForwardText;

    [HideInInspector] public bool isGameStarted = false;
    public GameObject gameStartButton;

    private Vector3 labelSize;

    public GameObject[] TutorialWindows;

    private void Awake()
    {
        currHealth = startingHealth;
        gameStartButton.SetActive(false);
        UpdateHealth(0);

        foreach (GameObject window in TutorialWindows)
        {
            window.SetActive(false);
        }
        TutorialWindows[0].SetActive(true);
    }

    private void Start(){
        Time.timeScale = 1f;
        instance = this;
        energyText.text = "0";
        labelSize = energyText.transform.localScale;
        UpdateEnergy(100);
    }

    public void UpdateEnergy(int amount){
        currentEnergy += amount;
        energyText.text = "" + currentEnergy;

        energyText.transform.DOPunchScale(Vector3.one * labelEffect, 0.2f, 0, 0).OnComplete(() => {
            //resetting in case multiple machines do the effect at the same time
            energyText.transform.localScale = labelSize;
        });
    }

    public void UpdateHealth(int amount)
    {
        currHealth += amount;
        if (currHealth <= 0)
        {
            isLighthouseDestroyed = true;
            inGameScreen.SetActive(false);
            loseScreen.SetActive(true);
        }
        healthText.text = currHealth.ToString();
    }

    public void StartGame()
    {
        isGameStarted = true;

        // disabling the tutorial windows once again just in case
        foreach (GameObject window in TutorialWindows)
        {
            window.SetActive(false);
        }
    }

    public void FastForward()
    {
        if (Time.timeScale > 1)
        {
            FastForwardText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            FastForwardText.gameObject.SetActive(true);
            Time.timeScale = 2f;
        }
            
    }
}
