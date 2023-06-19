using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class BuildCategory{
    public string category;
    public Outline buttonOutline;
}

[Serializable]
public class BuildItem{
    public GameObject item;
    public Vector3Int size;
    public int category;
    
    public int cost;
    public string description;
    public Sprite image;

    [Tooltip("No tags means you can place anywhere")]
    public List<string> targetTags;

    [HideInInspector] public GameObject uiButton;
    [HideInInspector] public GameObject placeholder;
    [HideInInspector] public GameObject usedPlatform;
}

public class BuildManager : MonoBehaviour{

    [SerializeField] private BuildCategory[] buildCategories;
    [SerializeField] private BuildItem[] buildItems;

    [SerializeField] private RectTransform buildBar;
    [SerializeField] private GameObject buildButton;

    [SerializeField] private GameObject grid;
    [SerializeField] private float snappingStep;

    [SerializeField] private Material placeholderMaterialGreen;
    [SerializeField] private Material placeholderMaterialRed;

    [SerializeField] private LayerMask buildLayers;

    [SerializeField] private GameObject cancelTip;

    [SerializeField] ParticleSystem smokeEffect;
    [SerializeField] ParticleSystem waterEffect;

    private bool canBuild = true;

    private int selectedCategory = 0;
    
    private int currentlyPlacing = -1;

    int tutorialStep = 0;

    private void Start(){
        cancelTip.SetActive(false);
        
        InitializeBuildItems();
        //SelectCategory(selectedCategory);
        
        // tutorial sequence:
        // 0 - Build a platform, then unlock energy buildings
        // 1 - Build energy building, then unlock towers
        // 2 - Build tower
        //foreach (BuildCategory category in buildCategories)
        //{
        //    category.buttonOutline.gameObject.SetActive(false);
        //}
        //buildCategories[0].buttonOutline.gameObject.SetActive(true);
    }

    private void InitializeBuildItems(){
        for(int i = 0; i < buildItems.Length; i++){
            GameObject newButton = Instantiate(buildButton, buildBar);
            BuildButton buildButtonController = newButton.GetComponent<BuildButton>();
            
            buildButtonController.Initialize(buildItems[i].image, buildItems[i].cost, buildItems[i].description, i, this);

            buildItems[i].uiButton = newButton;

            GameObject placeholder = Instantiate(buildItems[i].item);
            SetMaterials(placeholder, placeholderMaterialGreen);

            Collider col = placeholder.GetComponent<Collider>();

            if(col != null)
                col.enabled = false;
        
            if (placeholder.GetComponent<MonoBehaviour>())
            {
                placeholder.SendMessage("SetAsPlaceholder");
            }

            placeholder.SetActive(false);

            buildItems[i].placeholder = placeholder;
        }
    }

    private void Update(){
        if(canBuild)
            UpdateBuild();
    }

    private void UpdateBuild(){
        if(currentlyPlacing != -1){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            //ray from mouse position
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, buildLayers)){

                //check if mouse is over object
                if(hit.collider != null){
                    Vector3 pos = hit.point;
                    
                    pos /= snappingStep;
                    pos = new Vector3(Mathf.Round(pos.x), pos.y, Mathf.Round(pos.z));
                    pos *= snappingStep;
                    //pos.y = Mathf.Ceil(pos.y);

                    buildItems[currentlyPlacing].placeholder.transform.position = pos;

                    bool canPlace = buildItems[currentlyPlacing].targetTags.Count == 0;
                    canPlace |= buildItems[currentlyPlacing].targetTags.Contains(hit.collider.gameObject.tag);
                    
                    SetMaterials(buildItems[currentlyPlacing].placeholder, canPlace ? placeholderMaterialGreen : placeholderMaterialRed);

                    //try placing
                    if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()){
                        if(canPlace && buildItems[currentlyPlacing].cost <= GameManager.instance.currentEnergy)
                        {
                            GameManager.instance.UpdateEnergy(-buildItems[currentlyPlacing].cost);

                            TutorialSequence();

                            if (buildItems[currentlyPlacing].targetTags.Contains("Platform"))
                            {
                                Instantiate(smokeEffect.gameObject, pos, Quaternion.identity);
                                PlaceObjectOnPlatform(hit, pos);
                            }
                            else if (!buildItems[currentlyPlacing].targetTags.Contains("Platform"))
                            {
                                Instantiate(waterEffect.gameObject, pos, Quaternion.identity);
                                ConfirmPlaceBuildItem(pos);
                            }
                        }
                        else
                        { 
                            //shake and do warning
                            buildItems[currentlyPlacing].placeholder.transform
                                .DOPunchRotation(Random.insideUnitSphere.normalized * 40, 0.25f, 2, 0.5f).OnComplete(
                                    () => {
                                        buildItems[currentlyPlacing].placeholder.transform.rotation =
                                            buildItems[currentlyPlacing].item.transform.rotation;
                                    });
                        }
                    }
                }
            }
            
            if(Input.GetButtonDown("Cancel"))
                CancelItemPlacement();
        }
    }

    private void TutorialSequence()
    {
        if (tutorialStep == 0)
        {
            tutorialStep++;
            //buildCategories[1].buttonOutline.gameObject.SetActive(true);
            GameManager.instance.TutorialWindows[0].SetActive(false);
            GameManager.instance.TutorialWindows[1].SetActive(true);
        }
        else if (tutorialStep == 1)
        {
            tutorialStep++;
            //buildCategories[2].buttonOutline.gameObject.SetActive(true);
            GameManager.instance.TutorialWindows[1].SetActive(false);
            GameManager.instance.TutorialWindows[2].SetActive(true);
        }
        else if (tutorialStep == 2)
        {
            tutorialStep++;
            GameManager.instance.gameStartButton.SetActive(true);
            GameManager.instance.TutorialWindows[2].SetActive(false);
            GameManager.instance.TutorialWindows[3].SetActive(true);

        }
    }

    private void PlaceObjectOnPlatform(RaycastHit hit, Vector3 pos)
    {
        buildItems[currentlyPlacing].item.GetComponent<AllowToBuild>().takenPlatform = hit.collider.gameObject;
        ConfirmPlaceBuildItem(pos);
        hit.collider.gameObject.layer = 9;
        hit.collider.gameObject.tag = "UsedPlatform";
    }

    private void ConfirmPlaceBuildItem(Vector3 position){
        
        Quaternion rotation = buildItems[currentlyPlacing].item.transform.rotation;
        GameObject newBuildItem = Instantiate(buildItems[currentlyPlacing].item, position, rotation);
        newBuildItem.transform.DOPunchScale(Vector3.one * 0.3f, 0.25f, 2, 0.5f);
        
        CancelItemPlacement();
    }

    private void CancelItemPlacement(){
        buildItems[currentlyPlacing].placeholder.SetActive(false);
        currentlyPlacing = -1;
        
        cancelTip.SetActive(false);
    }

    public void ToggleGrid(bool show){
        grid.SetActive(show);
    }

    public void SelectCategory(int category){
        selectedCategory = category;

        for (int i = 0; i < buildCategories.Length; i++)
        {
            buildCategories[i].buttonOutline.enabled = i == selectedCategory;
        }

        //for (int i = 0; i < buildItems.Length; i++){
        //    buildItems[i].uiButton.SetActive(buildItems[i].category == selectedCategory);
        //}
    }

    public void PlaceBuildItem(int item){
        if(currentlyPlacing != -1)
            CancelItemPlacement();
        
        BuildItem buildItem = buildItems[item];
        buildItem.placeholder.SetActive(true);
        currentlyPlacing = item;
        
        cancelTip.SetActive(true);
    }

    private void SetMaterials(GameObject target, Material mat){
        foreach(Renderer rend in target.GetComponentsInChildren<Renderer>()){
            Material[] mats = rend.materials;
                
            for(int m = 0; m < mats.Length; m++){
                mats[m] = mat;
            }

            rend.materials = mats;
        }
    }
}
