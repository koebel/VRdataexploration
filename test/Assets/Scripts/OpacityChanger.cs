using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class OpacityChanger : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    // Timeline Interactor
    public GameObject photo;

    private float minVal = 0f;
    private float maxVal = 1f;
    private float opacity = 1f;

    // values range between -1 and 1
    private float x_move = 0.0f;
    private float x_current = 0.0f;
    private float movement;

    private bool touched = false;

    private Color col;


    void Start() {
        // assign parent objects of timeline interactor
        col = photo.GetComponent<Renderer>().material.color;
        col.a = opacity;
        photo.GetComponent<Renderer>().material.color = col;
    }


    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    void FixedUpdate() {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        // Checks for Touchpad touch, each touch       
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {

            /*
            if (device.GetAxis().x < 0.0 && opacity > minVal)
            {
                opacity -= 0.3f;
            }
            if (device.GetAxis().x > 0.0 && opacity < maxVal)
            {
                opacity += 0.3f;
            }
            col.a = opacity;
            */

            col.a = 0.5f;
            photo.GetComponent<Renderer>().material.color = col;
            /*
            if (!touched) {
                touched = true;
                //x_move = device.GetAxis().x;
            }

            else {
                x_current = device.GetAxis().x;
                movement = x_move - x_current;
                Debug.Log("movement" + movement);
                if (movement < 0 && opacity > minVal) {
                    opacity -= 0.3f;
                }
                if (movement > 0 && opacity > maxVal)
                {
                    opacity += 0.3f;
                }
                //opacity += (movement / 2) + 0.5f;

                Debug.Log("opacity" + opacity);

                col.a = opacity;
                col.a = opacity;
                photo.GetComponent<Renderer>().material.color = col;

                x_move = x_current;

            }
            */
        }

        // reset x_move to zero when touch is released
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            /*
            x_move = 0.0f;
            touched = false;
            */
            col.a = 1f;
            photo.GetComponent<Renderer>().material.color = col;
        }   
    }
}
