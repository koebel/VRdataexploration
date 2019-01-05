using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ScaleInteract : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    // Timeline Interactor
    public GameObject controller;
    public GameObject camrig;
    public GameObject zoomInteractor;
    public GameObject cube;

    // child objects
    private GameObject top;
    private GameObject bottom;
    private GameObject plus;
    private GameObject minus;

    //Scale Factor
    public float scaleFactor = 2;
    private float currentScaleFactor;
    private bool scalingActive = true;

    // materials for main menu
    public Material standardMaterial;
    public Material selectedMaterial;

    //Marker
    private GameObject marker;
    public float markerSize = 0.05f;
    private float markerRadiusFactor = 0.265f;
    private bool touched = false;

    // y_move measures the movement on the trackpad
    // values range between -1 and 1
    private float y_move = 0.0f;
    private float y_current = 0.0f;
    private float movement;
    public float speed = 0.1f;

    private float camrigX;
    private float camrigY;
    private float camrigZ;

    void Start() {
        currentScaleFactor = 1f * scaleFactor * scaleFactor;

        // find child objects of zoom interactor 
        top = zoomInteractor.transform.Find("zoom-top").gameObject;
        top = top.transform.Find("default").gameObject;

        bottom = zoomInteractor.transform.Find("zoom-bottom").gameObject;
        bottom = bottom.transform.Find("default").gameObject;

        plus = zoomInteractor.transform.Find("zoom-plus").gameObject;
        plus = plus.transform.Find("default").gameObject;

        minus = zoomInteractor.transform.Find("zoom-minus").gameObject;
        minus = minus.transform.Find("default").gameObject;

        top.GetComponent<MeshRenderer>().material = standardMaterial;
        bottom.GetComponent<MeshRenderer>().material = selectedMaterial;

        // create Marker that indicates input position on filter
        marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Collider c = marker.GetComponent<Collider>();
        c.enabled = false;
        marker.SetActive(false);

        // set position of marker
        marker.transform.parent = zoomInteractor.transform;
        marker.transform.localScale = new Vector3(markerSize, markerSize, markerSize);
        marker.transform.localPosition = new Vector3(-0.275f, 0.1f, 0.275f);
    }


    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    void FixedUpdate() {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        /*
        if (scalingActive)
        {
        }
        */

        // Checks for Touchpad touch swipe  
        /*
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (!touched) {
                touched = true;
                y_move = device.GetAxis().y;
            }

            else {
                y_current = device.GetAxis().y;
                movement = y_move - y_current;
                scaleFactor += movement * speed;
                //Debug.Log("Movement " + movement);
                Debug.Log("Scalefactor " + scaleFactor);

                // set scale of camrig, for the moment ignore values below 0
                if (scaleFactor > 0)
                {
                    Transform transform = camrig.transform;
                    camrig.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                }

                y_move = y_current;                
                
            }
        }
        */

        
        // set position of pointer on filter menu on Touchpad Touch
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {

            if (!touched)
            {
                // set position before activating marker
                marker.transform.localPosition = new Vector3(device.GetAxis().x * markerRadiusFactor * (-1) - 0.275f, 0.1f, (device.GetAxis().y * markerRadiusFactor * (-1) + 0.275f));
                marker.SetActive(true);
                touched = true;
            }
            else
            {
                marker.transform.localPosition = new Vector3(device.GetAxis().x * markerRadiusFactor * (-1) - 0.275f, 0.1f, (device.GetAxis().y * markerRadiusFactor * (-1) + 0.275f));
            }

            // set color of buttons when active
            // plus
            if (device.GetAxis().y > 0)
            {
                bottom.GetComponent<MeshRenderer>().material = standardMaterial;

                if (currentScaleFactor < 1025)
                {
                    top.GetComponent<MeshRenderer>().material = selectedMaterial;
                }
                else {
                    top.GetComponent<MeshRenderer>().material = standardMaterial;
                }
            }
            // minus
            else if (device.GetAxis().y < 0)
            {
                top.GetComponent<MeshRenderer>().material = standardMaterial;

                if (currentScaleFactor > 1)
                {
                    bottom.GetComponent<MeshRenderer>().material = selectedMaterial;
                }
                else
                {
                    bottom.GetComponent<MeshRenderer>().material = standardMaterial;
                }
            }
        }
        

        // Checks for Touchpad clicks for stepwise scaling      
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            camrigX = camrig.transform.localPosition.x;
            camrigY = camrig.transform.localPosition.y;
            camrigZ = camrig.transform.localPosition.z;

            //scale up (zoom out)
            if (device.GetAxis().y > 0 && currentScaleFactor < 1025)
            {
                currentScaleFactor *= scaleFactor;

                Debug.Log("Scalefactor " + currentScaleFactor);

                Transform transform = cube.transform;
                camrig.transform.localScale = new Vector3(currentScaleFactor, currentScaleFactor, currentScaleFactor);
            }

            // scale down (zoom in)
            else if (device.GetAxis().y < 0 && currentScaleFactor > 1)
            {
                currentScaleFactor /= scaleFactor;

                Debug.Log("Scalefactor " + currentScaleFactor);

                Transform transform = cube.transform;
                camrig.transform.localScale = new Vector3(currentScaleFactor, currentScaleFactor, currentScaleFactor);
            }
        }

        // reset y_move to zero when touch is released
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            y_move = 0.0f;

            // hide marke on touch up
            marker.SetActive(false);
            top.GetComponent<MeshRenderer>().material = standardMaterial;
            bottom.GetComponent<MeshRenderer>().material = standardMaterial;
            touched = false;
        }   
    }
}
