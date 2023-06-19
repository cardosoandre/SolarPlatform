using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BuildingInformation : MonoBehaviour
{
    public static BuildingInformation Instance { get; private set; }

    private CanvasGroup canvasGroup;
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
        canvasGroup = GetComponent<CanvasGroup>();
        //gameObject.SetActive(false);
    }


    void Update()
    {
        
    }

    public void Fade(bool state)
    {
        canvasGroup.DOFade(state ? 1 : 0, .1f);
    }
    public void ChangeInformation(int cost, string description)
    {
        costText.text = cost.ToString();
        descriptionText.text = description;
    }
}
