using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInformation : MonoBehaviour
{
    public static BuildingInformation Instance { get; private set; }

    [SerializeField] Text costText;
    [SerializeField] Text descriptionText;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
    }


    void Update()
    {
        
    }

    public void ChangeInformation(int cost, string description)
    {
        costText.text = cost.ToString();
        descriptionText.text = description;
    }
}
