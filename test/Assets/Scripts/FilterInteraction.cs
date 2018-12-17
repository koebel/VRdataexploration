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
    
    // child objects
    private GameObject topLeft;
    private GameObject topRight;
    private GameObject bottomLeft;
    private GameObject bottomRight;
    private GameObject center;

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

    //Scale Factor
    private float scaleFactor;

    private bool filterActive = true;
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
        bottomLeft.GetComponent<MeshRenderer>().material = matType;
        center.GetComponent<MeshRenderer>().material = standardMaterial;


        scaleFactor = 1f;
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

        // Checks for Touchpad clicks for stepwise scaling      
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            camrigX = 0;
            camrigY = 0;
            camrigZ = 0;

            //scale up (zoom out)
            if (device.GetAxis().y > 0)
            {
                scaleFactor *= 2;

                Debug.Log("Scalefactor " + scaleFactor);

                if (scaleFactor < 1025)
                {
                    //Transform transform = cube.transform;
                    camrig.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                }
            }

            // scale down (zoom in)
            else
            {
                scaleFactor /= 2;
                //Debug.Log("Movement " + movement);
                Debug.Log("Scalefactor " + scaleFactor);

                // set scale of camrig, for the moment ignore values below 0
                
                if (scaleFactor > 0) {
                    // Transform transform = cube.transform;
                    camrig.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

                    // adjust cam hight
                    if (camrigY < 1) {
                        camrig.transform.localPosition = new Vector3(camrigX, 1.0f, camrigZ);
                    }
                }

                /*
                // set scale of cube, for the moment ignore values below 0
                if (scaleFactor > 0)
                {
                    Transform transform = cube.transform;
                    cube.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                }
                */
            }
        }




        // reset y_move to zero when touch is released
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            y_move = 0.0f;
            touched = false;
        }   
    }
}
