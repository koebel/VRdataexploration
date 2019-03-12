using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

using CollectionDataHandlingSpace;
using DataVisualisationSpace;


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class FilterInteract : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    // Filter Interactor
    public GameObject filterInteractor;
    private GameObject controller;

    public float centerRadius = 0.4f;

    // child objects
    private GameObject filter;
    private GameObject filterButton;
    private GameObject apply;
    private GameObject applyButton;
    private GameObject back;
    private GameObject backButton;

    private GameObject mainMenu;
    private GameObject metaMenu;
    private GameObject subMenuRegion;

    private GameObject mainTimespan;
    private GameObject mainRegion;
    private GameObject mainType;
    private GameObject mainExhibition;

    private GameObject metaSelectAll;
    private GameObject metaSelectNone;
    private GameObject metaSettings;

    private GameObject subRegionAmerica;
    private GameObject subRegionAfrica;
    private GameObject subRegionEurope;
    private GameObject subRegionAsia;
    private GameObject subRegionOzeania;

    private GameObject topLeft;
    private GameObject topRight;
    private GameObject bottomLeft;
    private GameObject bottomRight;
    private GameObject center;

    // Materials
    public Material standardMaterial;
    public Material standardCenterMaterial;
    public Material selectedMaterial;
    public Material selectedCenterMaterial;
    public Material inactiveMaterial;

    private Material matTimespan;
    private Material matRegion;
    private Material matType;
    private Material matExhibition;
    private Material matApply;

    private Material matTimespanSelected;
    private Material matRegionSelected;
    private Material matTypeSelected;
    private Material matExhibitionSelected;
    private Material matApplySelected;

    //selections
    private bool applySelected = false;

    private bool timespanSelected = false;
    private bool regionSelected = false;
    private bool typeSelected = false;
    private bool exhibitionSelected = false;

    private bool subRegionAmericaSelected = true;
    private bool subRegionAfricaSelected = true;
    private bool subRegionEuropeSelected = true;
    private bool subRegionAsiaSelected = true;
    private bool subRegionOzeaniaSelected = true;

    private bool filterActive = false;
    private float currAngle = 0.0f;

    //Marker
    private GameObject marker;
    public float markerSize = 0.05f;
    private float markerRadiusFactor = 0.265f;
    private float markerRadiusFactorMenu = 0.665f;
    private float markerOffsetHorizontal = -0.275f;
    private float markerOffsetVertical = 0.275f;
    private float markerOffsetDepth = 0.05f;
    private bool touched = false;

    // components
    private DataVisualisationSpace.DataVisualisation dv;


    void Start() {

        dv = GetComponent<DataVisualisation>();

        // assign parent objects of filter interactor
        controller = filterInteractor.transform.parent.gameObject;

        // find child objects of filter interactor
        // Filter & Apply
        filter = filterInteractor.transform.Find("Filter").gameObject;
        filterButton = filter.transform.Find("btn-filter").gameObject;
        filterButton = filterButton.transform.Find("default").gameObject;

        apply = filterInteractor.transform.Find("Apply").gameObject;
        applyButton = apply.transform.Find("btn-apply").gameObject;
        applyButton = applyButton.transform.Find("default").gameObject;

        back = filterInteractor.transform.Find("Back").gameObject;
        backButton = back.transform.Find("btn-back").gameObject;
        backButton = backButton.transform.Find("default").gameObject;

        // Main Menu
        mainMenu = filterInteractor.transform.Find("MainMenu").gameObject;

        mainTimespan = mainMenu.transform.Find("segment-4-1").gameObject;
        mainTimespan = mainTimespan.transform.Find("default").gameObject;

        mainRegion = mainMenu.transform.Find("segment-4-2").gameObject;
        mainRegion = mainRegion.transform.Find("default").gameObject;

        mainType = mainMenu.transform.Find("segment-4-3").gameObject;
        mainType = mainType.transform.Find("default").gameObject;

        mainExhibition = mainMenu.transform.Find("segment-4-4").gameObject;
        mainExhibition = mainExhibition.transform.Find("default").gameObject;

        // Meta Menu
        metaMenu = filterInteractor.transform.Find("MetaMenu").gameObject;

        metaSettings = metaMenu.transform.Find("segment-bottom").gameObject;
        metaSettings = metaSettings.transform.Find("default").gameObject;

        // Submenu Region
        subMenuRegion = filterInteractor.transform.Find("SubMenuRegion").gameObject;

        subRegionAmerica = subMenuRegion.transform.Find("segment-5-1").gameObject;
        subRegionAmerica = subRegionAmerica.transform.Find("default").gameObject;

        subRegionAfrica = subMenuRegion.transform.Find("segment-5-2").gameObject;
        subRegionAfrica = subRegionAfrica.transform.Find("default").gameObject;

        subRegionEurope = subMenuRegion.transform.Find("segment-5-3").gameObject;
        subRegionEurope = subRegionEurope.transform.Find("default").gameObject;

        subRegionAsia = subMenuRegion.transform.Find("segment-5-4").gameObject;
        subRegionAsia = subRegionAsia.transform.Find("default").gameObject;

        subRegionOzeania = subMenuRegion.transform.Find("segment-5-5").gameObject;
        subRegionOzeania = subRegionOzeania.transform.Find("default").gameObject;


        // load Materials
        filterButton.GetComponent<MeshRenderer>().material = standardCenterMaterial;
        applyButton.GetComponent<MeshRenderer>().material = standardCenterMaterial;
        backButton.GetComponent<MeshRenderer>().material = standardCenterMaterial;

        mainTimespan.GetComponent<MeshRenderer>().material = standardMaterial;
        mainRegion.GetComponent<MeshRenderer>().material = standardMaterial;
        mainType.GetComponent<MeshRenderer>().material = standardMaterial;
        mainExhibition.GetComponent<MeshRenderer>().material = standardMaterial;

        //metaSelectAll.GetComponent<MeshRenderer>().material = inactiveMaterial;
        //metaSelectNone.GetComponent<MeshRenderer>().material = inactiveMaterial;
        metaSettings.GetComponent<MeshRenderer>().material = inactiveMaterial;

        subRegionAmerica.GetComponent<MeshRenderer>().material = selectedMaterial;
        subRegionAfrica.GetComponent<MeshRenderer>().material = selectedMaterial;
        subRegionEurope.GetComponent<MeshRenderer>().material = selectedMaterial;
        subRegionAsia.GetComponent<MeshRenderer>().material = selectedMaterial;
        subRegionOzeania.GetComponent<MeshRenderer>().material = selectedMaterial;

        // hide menus
        apply.SetActive(false);
        back.SetActive(false);
        mainMenu.SetActive(false);
        metaMenu.SetActive(false);
        subMenuRegion.SetActive(false);

        // create Marker that indicates input position on filter
        marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Collider c = marker.GetComponent<Collider>();
        c.enabled = false;
        //MeshRenderer r = marker.GetComponent<MeshRenderer>();
        //r.material.color = selectedMaterial;
        //r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        marker.SetActive(false);

        // set position
        marker.transform.parent = filterInteractor.transform;
        marker.transform.localScale = new Vector3(markerSize, markerSize, markerSize);
        marker.transform.localPosition = new Vector3(markerOffsetHorizontal, markerOffsetDepth, markerOffsetVertical);
    }


    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    void FixedUpdate() {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        // calculate angle of touchpoint on trackpad
        currAngle = Mathf.Atan2(device.GetAxis().x, device.GetAxis().y) * Mathf.Rad2Deg + 180;
        // currAngle contains a value from 0 to 360
        // 60 to 300 are positions in the Menu

        currAngle = (currAngle + 300) % 360;
        // transfor value so that values from 0 to 240 apply to the Menu
        // and values larger than 240 applies to the Meta Menu

        // show filter menu
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            // display filter menu
            if (!filterActive)
            {
                filter.SetActive(false);
                apply.SetActive(true);
                mainMenu.SetActive(true);
                metaMenu.SetActive(true);
                filterActive = true;
            }
            // hide filter menu
            else
            {
                filter.SetActive(true);
                apply.SetActive(false);
                back.SetActive(false);
                mainMenu.SetActive(false);
                metaMenu.SetActive(false);
                filterActive = false;
            }
        }

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            // set position of pointer on filter menu on Touchpad Touch
            if (!touched)
            {
                // set position before activating marker, differenciate between filter active and not
                if (filterActive) {
                    marker.transform.localPosition = new Vector3(device.GetAxis().x * markerRadiusFactorMenu * (-1) + markerOffsetHorizontal, markerOffsetDepth, (device.GetAxis().y * markerRadiusFactorMenu * (-1) + markerOffsetVertical));
                }
                else {
                    marker.transform.localPosition = new Vector3(device.GetAxis().x * markerRadiusFactor * (-1) + markerOffsetHorizontal, markerOffsetDepth, (device.GetAxis().y * markerRadiusFactor * (-1) + markerOffsetVertical));
                }
                
                marker.SetActive(true);
                touched = true;
            }
            else
            {
                // set position before activating marker, differenciate between filter active and not
                if (filterActive)
                {
                    marker.transform.localPosition = new Vector3(device.GetAxis().x * markerRadiusFactorMenu * (-1) + markerOffsetHorizontal, markerOffsetDepth, (device.GetAxis().y * markerRadiusFactorMenu * (-1) + markerOffsetVertical));

                    // TODO change color of apply button if curser is in this region?
                    if (Mathf.Sqrt(Mathf.Abs(device.GetAxis().x) * Mathf.Abs(device.GetAxis().x) + Mathf.Abs(device.GetAxis().y) * Mathf.Abs(device.GetAxis().y)) < centerRadius)
                    {
                        applyButton.GetComponent<MeshRenderer>().material = selectedCenterMaterial;
                    }
                    else {
                        applyButton.GetComponent<MeshRenderer>().material = standardCenterMaterial;
                    }
                }

                else
                {
                    marker.transform.localPosition = new Vector3(device.GetAxis().x * markerRadiusFactor * (-1) + markerOffsetHorizontal, markerOffsetDepth, (device.GetAxis().y * markerRadiusFactor * (-1) + markerOffsetVertical));
                }
            }
        }

        // Check for Touchpad input  
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) {
               
            if (filterActive)
            {
                //check for input center
                if (Mathf.Sqrt(Mathf.Abs(device.GetAxis().x) * Mathf.Abs(device.GetAxis().x) + Mathf.Abs(device.GetAxis().y) * Mathf.Abs(device.GetAxis().y)) < centerRadius)
                {

                    Debug.Log("Distance: " + Mathf.Sqrt(Mathf.Abs(device.GetAxis().x) * Mathf.Abs(device.GetAxis().x) + Mathf.Abs(device.GetAxis().y) * Mathf.Abs(device.GetAxis().y)));
                    // update selection 
                    //Debug.Log("center clicked");

                    if (regionSelected)
                    {
                        regionSelected = false;
                        Debug.Log("back to main menu");

                        // hide sub menu region & update material
                        mainMenu.SetActive(true);
                        subMenuRegion.SetActive(false);
                        apply.SetActive(true);
                        back.SetActive(false);
                    }

                    // no submenu selected
                    else
                    {
                        // go back to filter, hide menu
                        filter.SetActive(true);
                        apply.SetActive(false);
                        mainMenu.SetActive(false);
                        metaMenu.SetActive(false);
                        filterActive = false;

                        // apply selection to data visualisation
                        dv.resetDataVisualisation();
                        CollectionDataHandling.CollectionData.selectItemsByRegion(subRegionAmericaSelected, subRegionAfricaSelected, subRegionEuropeSelected, subRegionAsiaSelected, subRegionOzeaniaSelected);
                        dv.applySelection();
                    }
                }

                // input outside center area
                else {
                    //check for input submenus
                    /*
                    if (timespanSelected)
                    {
                        timespanSelected = false;
                        // DO SOMETHING
                    }
                    */
                    if (regionSelected)
                    {
                        if (currAngle > 0 && currAngle < 48) {
                            Debug.Log("America clicked");
                            subRegionAmericaSelected = !subRegionAmericaSelected;

                            // update material
                            if (subRegionAmericaSelected)
                            {
                                subRegionAmerica.GetComponent<MeshRenderer>().material = selectedMaterial;
                            }
                            else
                            {
                                subRegionAmerica.GetComponent<MeshRenderer>().material = standardMaterial;
                            }
                        }
                        else if (currAngle > 48 && currAngle < 96)
                        {
                            Debug.Log("Africa clicked");
                            subRegionAfricaSelected = !subRegionAfricaSelected;

                            // update material
                            if (subRegionAfricaSelected)
                            {
                                subRegionAfrica.GetComponent<MeshRenderer>().material = selectedMaterial;
                            }
                            else
                            {
                                subRegionAfrica.GetComponent<MeshRenderer>().material = standardMaterial;
                            }
                        }
                        else if (currAngle > 96 && currAngle < 144)
                        {
                            Debug.Log("Europe clicked");
                            subRegionEuropeSelected = !subRegionEuropeSelected;

                            // update material
                            if (subRegionEuropeSelected)
                            {
                                subRegionEurope.GetComponent<MeshRenderer>().material = selectedMaterial;
                            }
                            else
                            {
                                subRegionEurope.GetComponent<MeshRenderer>().material = standardMaterial;
                            }
                        }
                        else if (currAngle > 144 && currAngle < 192)
                        {
                            Debug.Log("Asia clicked");
                            subRegionAsiaSelected = !subRegionAsiaSelected;

                            // update material
                            if (subRegionAsiaSelected)
                            {
                                subRegionAsia.GetComponent<MeshRenderer>().material = selectedMaterial;
                            }
                            else
                            {
                                subRegionAsia.GetComponent<MeshRenderer>().material = standardMaterial;
                            }
                        }
                        else if (currAngle > 192 && currAngle < 240)
                        {
                            Debug.Log("Oceania clicked");
                            subRegionOzeaniaSelected = !subRegionOzeaniaSelected;

                            // update material
                            if (subRegionOzeaniaSelected)
                            {
                                subRegionOzeania.GetComponent<MeshRenderer>().material = selectedMaterial;
                            }
                            else
                            {
                                subRegionOzeania.GetComponent<MeshRenderer>().material = standardMaterial;
                            }
                        }
                    }

                    /*
                    if (typeSelected)
                    {
                        typeSelected = false;
                        // DO SOMETHING
                    }
                    

                    if (exhibitionSelected)
                    {
                        exhibitionSelected = false;
                        // DO SOMETHING
                    }
                    */

                    //check for input metamenu (bottom) --> do not implement yet
                    if (currAngle > 240)
                    {
                        // reset all selections
                        CollectionDataHandlingSpace.CollectionDataHandling.CollectionData.deselectAll();
                        
                        timespanSelected = false;
                        regionSelected = false;
                        typeSelected = false;
                        exhibitionSelected = false;

                        subRegionAmericaSelected = false;
                        subRegionAfricaSelected = false;
                        subRegionEuropeSelected = false;
                        subRegionAsiaSelected = false;
                        subRegionOzeaniaSelected = false;

                        subRegionAmerica.GetComponent<MeshRenderer>().material = standardMaterial;
                        subRegionAfrica.GetComponent<MeshRenderer>().material = standardMaterial;
                        subRegionEurope.GetComponent<MeshRenderer>().material = standardMaterial;
                        subRegionAsia.GetComponent<MeshRenderer>().material = standardMaterial;
                        subRegionOzeania.GetComponent<MeshRenderer>().material = standardMaterial;

                        // apply reset data visualisation
                        dv.resetDataVisualisation();
                        dv.applyDataVisualisation();
                    }

                    //check for input mainsections

                    // timespan
                    else if (currAngle > 0 && currAngle < 60)
                    {
                        timespanSelected = !timespanSelected;

                        // update material
                        if (!timespanSelected)
                        {
                            mainTimespan.GetComponent<MeshRenderer>().material = selectedMaterial;
                        }
                        else
                        {
                            mainTimespan.GetComponent<MeshRenderer>().material = standardMaterial;
                        }
                    }

                    // region
                    else if (currAngle > 60 && currAngle < 120)
                    {
                        // show sub menu region & update material
                        subMenuRegion.SetActive(true);
                        mainMenu.SetActive(false);
                        back.SetActive(true);
                        apply.SetActive(false);
                        regionSelected = true;
                    }

                    // type
                    else if (currAngle > 120 && currAngle < 180)
                    {
                        typeSelected = !typeSelected;

                        // update material
                        if (typeSelected)
                        {
                            mainType.GetComponent<MeshRenderer>().material = selectedMaterial;
                        }
                        else
                        {
                            mainType.GetComponent<MeshRenderer>().material = standardMaterial;
                        }
                    }

                    // exhibition
                    else if (currAngle > 180 && currAngle < 240)
                    {
                        exhibitionSelected = !exhibitionSelected;

                        // update material
                        if (exhibitionSelected)
                        {
                            mainExhibition.GetComponent<MeshRenderer>().material = selectedMaterial;
                        }
                        else
                        {
                            mainExhibition.GetComponent<MeshRenderer>().material = standardMaterial;
                        }
                    }
                }
            }

            // filter menu not active
            else
            {
                filter.SetActive(false);
                apply.SetActive(true);
                mainMenu.SetActive(true);
                metaMenu.SetActive(true);
                filterActive = true;
            }
        }

        // hide marker if touch is released
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            touched = false;
            applyButton.GetComponent<MeshRenderer>().material = standardCenterMaterial;
            marker.SetActive(false);
        }   
    }
}
