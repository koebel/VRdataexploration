﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class FilterInteraction : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    // Filter Interactor
    public GameObject filterInteractor;
    private GameObject controller;
    private GameObject camrig;

    public float centerRadius = 0.5f;

    // child objects
    private GameObject filter;
    private GameObject filterButton;
    private GameObject apply;
    private GameObject applyButton;

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
    public Material selectedMaterial;
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


    void Start() {
        // assign parent objects of filter interactor
        controller = filterInteractor.transform.parent.gameObject;
        camrig = controller.transform.parent.gameObject;

        // find child objects of filter interactor
        // Filter & Apply
        filter = filterInteractor.transform.Find("Filter").gameObject;
        filterButton = filter.transform.Find("btn-filter").gameObject;
        filterButton = filterButton.transform.Find("default").gameObject;

        apply = filterInteractor.transform.Find("Apply").gameObject;
        applyButton = apply.transform.Find("btn-apply").gameObject;
        applyButton = applyButton.transform.Find("default").gameObject;

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

        /*
        // find child objects of filter interactor old
        topLeft = filterInteractor.transform.Find("TopLeft").gameObject;
        topRight = filterInteractor.transform.Find("TopRight").gameObject;
        bottomLeft = filterInteractor.transform.Find("BottomLeft").gameObject;
        bottomRight = filterInteractor.transform.Find("BottomRight").gameObject;
        center = filterInteractor.transform.Find("Center").gameObject;
        */

        // load Materials
        /*
        matTimespan = (Material)Resources.Load("Experiments/Materials/experiment1", typeof(Material));
        matRegion = (Material)Resources.Load("Experiments/Materials/experiment2", typeof(Material));
        matType = (Material)Resources.Load("Experiments/Materials/experiment3", typeof(Material));
        matExhibition = (Material)Resources.Load("Experiments/Materials/experiment4", typeof(Material));
        matApply = (Material)Resources.Load("Materials/PinkMaterial", typeof(Material));

        topLeft.GetComponent<MeshRenderer>().material = matRegion;
        topRight.GetComponent<MeshRenderer>().material = matTimespan;
        bottomLeft.GetComponent<MeshRenderer>().material = matExhibition;
        bottomRight.GetComponent<MeshRenderer>().material = matType;
        center.GetComponent<MeshRenderer>().material = standardMaterial;
        */

        filterButton.GetComponent<MeshRenderer>().material = standardMaterial;
        applyButton.GetComponent<MeshRenderer>().material = standardMaterial;

        mainTimespan.GetComponent<MeshRenderer>().material = standardMaterial;
        mainRegion.GetComponent<MeshRenderer>().material = standardMaterial;
        mainType.GetComponent<MeshRenderer>().material = standardMaterial;
        mainExhibition.GetComponent<MeshRenderer>().material = standardMaterial;

        //metaSelectAll.GetComponent<MeshRenderer>().material = inactiveMaterial;
        //metaSelectNone.GetComponent<MeshRenderer>().material = inactiveMaterial;
        metaSettings.GetComponent<MeshRenderer>().material = inactiveMaterial;

        subRegionAmerica.GetComponent<MeshRenderer>().material = standardMaterial;
        subRegionAfrica.GetComponent<MeshRenderer>().material = standardMaterial;
        subRegionEurope.GetComponent<MeshRenderer>().material = standardMaterial;
        subRegionAsia.GetComponent<MeshRenderer>().material = standardMaterial;
        subRegionOzeania.GetComponent<MeshRenderer>().material = standardMaterial;

        // hide menus
        apply.SetActive(false);
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

        // show filter menu
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            // display filter menu
            if (!filterActive)
            {
                //filterInteractor.SetActive(true);
                filter.SetActive(false);
                apply.SetActive(true);
                mainMenu.SetActive(true);
                metaMenu.SetActive(true);
                filterActive = true;
            }
            // hide filter menu
            else
            {
                //filterInteractor.SetActive(false);
                filter.SetActive(true);
                apply.SetActive(false);
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
                // calculate angle of touchpoint on trackpad
                currAngle = Mathf.Atan2(device.GetAxis().x, device.GetAxis().y) * Mathf.Rad2Deg + 180;
                // currAngle contains a value from 0 to 360  
                // TODO might need multiplication by factor -1 for correction
                Debug.Log("Angle: " + currAngle);

                //check for input center --> go back to filter & apply selection to dataset
                if (Mathf.Sqrt(Mathf.Abs(device.GetAxis().x) * Mathf.Abs(device.GetAxis().x) + Mathf.Abs(device.GetAxis().y) + Mathf.Abs(device.GetAxis().y)) < centerRadius)
                {
                    // update selection 
                    applySelected = !applySelected;
                    // TODO apply selection to dataset

                    // hide filter menu
                    filter.SetActive(true);
                    apply.SetActive(false);
                    mainMenu.SetActive(false);
                    metaMenu.SetActive(false);
                    filterActive = false;

                    /*
                    // update material
                    if (applySelected)
                    {
                        center.GetComponent<MeshRenderer>().material = selectedMaterial;
                    }
                    else
                    {
                        center.GetComponent<MeshRenderer>().material = standardMaterial;
                    }
                    */
                }


                //check for input metamenu (bottom) --> do not implement yet
                //check for input submenus
                if (timespanSelected)
                {
                    // DO SOMETHING
                }

                else if (regionSelected)
                {
                    // DO SOMETHING
                }

                else if (typeSelected)
                {
                    // DO SOMETHING
                }

                else if (exhibitionSelected)
                {
                    // DO SOMETHING
                }

                else {
                    //check for input mainsections
                }


                /*                
                

                //top left
                //else if (device.GetAxis().x < 0 && device.GetAxis().y > 0)
                else if (currAngle > 90 && currAngle < 180)
                {
                    // update selection 
                    regionSelected = !regionSelected;

                    // update material
                    if (regionSelected)
                    {
                        topLeft.GetComponent<MeshRenderer>().material = selectedMaterial;
                    }
                    else {
                        topLeft.GetComponent<MeshRenderer>().material = standardMaterial;
                    }
                }

                //top right
                //else if (device.GetAxis().x > 0 && device.GetAxis().y > 0)
                else if (currAngle > 180 && currAngle < 270)
                {
                    // update selection 
                    timespanSelected = !timespanSelected;

                    // update material
                    if (timespanSelected)
                    {
                        topRight.GetComponent<MeshRenderer>().material = selectedMaterial;
                    }
                    else
                    {
                        topRight.GetComponent<MeshRenderer>().material = standardMaterial;
                    }
                }

                //bottom left
                //else if (device.GetAxis().x < 0 && device.GetAxis().y < 0)
                else if (currAngle < 90)
                {
                    // update selection 
                    exhibitionSelected = !exhibitionSelected;

                    // update material
                    if (exhibitionSelected)
                    {
                        bottomLeft.GetComponent<MeshRenderer>().material = selectedMaterial;
                    }
                    else
                    {
                        bottomLeft.GetComponent<MeshRenderer>().material = standardMaterial;
                    }
                }

                //top right
                //else if (device.GetAxis().x > 0 && device.GetAxis().y < 0)
                else if (currAngle > 270)
                {
                    // update selection 
                    typeSelected = !typeSelected;

                    // update material
                    if (typeSelected)
                    {
                        bottomRight.GetComponent<MeshRenderer>().material = selectedMaterial;
                    }
                    else
                    {
                        bottomRight.GetComponent<MeshRenderer>().material = standardMaterial;
                    }
                }
                */
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
            marker.SetActive(false);
        }   
    }
}
