using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour{

    [SerializeField] private Image image;
    [SerializeField] GameObject buildingInfoWindow;

    private BuildManager buildManager;
    private int buildItemIndex;

    private int itemCost;
    private string itemDescription;

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
        BuildingInformation.Instance.gameObject.SetActive(true);
    }

    public void HideInfoWindow()
    {
        BuildingInformation.Instance.gameObject.SetActive(false);
    }
}
