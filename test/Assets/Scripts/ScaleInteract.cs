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
    public GameObject cube;

    //Scale Factor
    private float scaleFactor;

    private bool scalingActive = true;
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
            camrigX = camrig.transform.localPosition.x;
            camrigY = camrig.transform.localPosition.y;
            camrigZ = camrig.transform.localPosition.z;

            //scale up (zoom out)
            if (device.GetAxis().y > 0)
            {
                scaleFactor *= 2;

                Debug.Log("Scalefactor " + scaleFactor);

                if (scaleFactor < 1025)
                {
                    Transform transform = cube.transform;
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
                    Transform transform = cube.transform;
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
