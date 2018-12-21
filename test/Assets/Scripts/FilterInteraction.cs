using UnityEngine;
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
    public float markerSize = 0.05f;
    private float filterMenuFactor = 0.25f;
    
    // child objects
    private GameObject topLeft;
    private GameObject topRight;
    private GameObject bottomLeft;
    private GameObject bottomRight;
    private GameObject center;
    private GameObject marker;

    // materials for main menu
    public Material standardMaterial;
    public Material selectedMaterial;

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
    private bool touched = false;
    private float currAngle = 0.0f;


    void Start() {
        // assign parent objects of filter interactor
        controller = filterInteractor.transform.parent.gameObject;
        camrig = controller.transform.parent.gameObject;

        // find child objects of filter interactor
        topLeft = filterInteractor.transform.Find("TopLeft").gameObject;
        topRight = filterInteractor.transform.Find("TopRight").gameObject;
        bottomLeft = filterInteractor.transform.Find("BottomLeft").gameObject;
        bottomRight = filterInteractor.transform.Find("BottomRight").gameObject;
        center = filterInteractor.transform.Find("Center").gameObject;

        // load Materials

        //standardMaterial = (Material)Resources.Load("Timeline/Materials/DefaultMaterial", typeof(Material));

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
        marker.transform.localPosition = new Vector3(0.0f, 1.11f, -0.3f);    
    }


    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    void FixedUpdate() {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        // show filter menu
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            // display filter
            if (!filterActive)
            {
                filterInteractor.SetActive(true);
                filterActive = true;
            }
            // hide filter
            else
            {
                filterInteractor.SetActive(false);
                filterActive = false;
            }
        }

        if (filterActive) {
            // set position of pointer on filter menu
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {

                if (!touched)
                {
                    // set position before activating marker
                    marker.transform.localPosition = new Vector3(device.GetAxis().x * filterMenuFactor, 1.11f, (device.GetAxis().y * filterMenuFactor - 0.3f));
                    marker.SetActive(true);
                }
                else {
                    marker.transform.localPosition = new Vector3(device.GetAxis().x * filterMenuFactor, 1.11f, (device.GetAxis().y * filterMenuFactor - 0.3f));
                }

                //calculate angle of touchpoint on trackpad
                currAngle = Mathf.Atan2(device.GetAxis().x, device.GetAxis().y) * Mathf.Rad2Deg + 180;
                // currAngle contains a value from 0 to 360              
            }

            // Checks for Touchpad input     
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                //center
                if (Mathf.Sqrt(Mathf.Abs(device.GetAxis().x) * Mathf.Abs(device.GetAxis().x) + Mathf.Abs(device.GetAxis().y) + Mathf.Abs(device.GetAxis().y)) < centerRadius) {

                    // update selection 
                    applySelected = !applySelected;

                    // update material
                    if (applySelected)
                    {
                        center.GetComponent<MeshRenderer>().material = selectedMaterial;
                    }
                    else
                    {
                        center.GetComponent<MeshRenderer>().material = standardMaterial;
                    }
                }

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
