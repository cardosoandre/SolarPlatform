using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler{

    Button myButton;

    [SerializeField] private Image image;
    [SerializeField] GameObject buildingInfoWindow;

    private BuildManager buildManager;
    private int buildItemIndex;

    private int itemCost;
    private string itemDescription;

    private void Start()
    {
        myButton = GetComponent<Button>();
    }

    public void Initialize(Sprite sprite, int cost, string description,int buildItemIndex, BuildManager buildManager){
        image.sprite = sprite;
        itemCost = cost;
        itemDescription = description;

        this.buildItemIndex = buildItemIndex;
        this.buildManager = buildManager;
    }

    public void SelectBuildItem(){
        buildManager.PlaceBuildItem(buildItemIndex);
    }

    public void ShowInfoWindow()
    {
        BuildingInformation.Instance.ChangeInformation(itemCost, itemDescription);
        BuildingInformation.Instance.Fade(true);
        //BuildingInformation.Instance.gameObject.SetActive(true);


    }

    public void HideInfoWindow()
    {
        //BuildingInformation.Instance.gameObject.SetActive(false);
        BuildingInformation.Instance.Fade(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!myButton.interactable)
            return;

        ShowInfoWindow();
        transform.DOComplete();
        transform.DOScale(1.2f, .1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!myButton.interactable)
            return;

        HideInfoWindow();
        transform.DOComplete();
        transform.DOScale(1, .1f);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        transform.DOComplete();
        transform.DOShakeScale(.2f, .5f, 20, 90, true);
    }
}
