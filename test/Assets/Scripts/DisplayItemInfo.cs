using UnityEngine.UI;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class DisplayItemInfo : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    private GameObject collectionItem;
    private Transform collectionItemParent;
    private GameObject collectionItemUI;
    private GameObject collectionItemBG;
    private Text text;

    // set Tag for collection item
    private string collectionItemTag = "CollectionItem";

    private bool infoActive = false;

    private Ray ray;
    private RaycastHit hit;
    private GameObject hitObject;
    public float rayDistance = 15.0f;


    void Start () {

    }


    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        // shoot ray
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            ray = new Ray();
            ray.origin = trackedObj.transform.position;
            ray.direction = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // hit object
                hitObject = hit.collider.gameObject;

                // TODO check if UI is already visible, only call the following code once

                if (hitObject.tag == collectionItemTag)
                {
                    collectionItem = hitObject;
                    //Debug.Log(collectionItem.name);

                    // check if object has parent
                    if (collectionItem.transform.parent != null) {
                        //Debug.Log(collectionItem.transform.parent.name);

                        // set collection item 
                        collectionItemParent = collectionItem.transform.parent;
                        collectionItemUI = collectionItemParent.transform.Find("UI").gameObject;
                        collectionItemBG = collectionItemParent.transform.Find("background").gameObject;
                        collectionItemUI.SetActive(true);

                        // TODO: make Background semitransparent
                        collectionItemBG.SetActive(true);
                    }
                }
                
                // hide UI when ray hits object of different type
                else
                {
                    collectionItemUI.SetActive(false);
                    collectionItemBG.SetActive(false);
                    collectionItem = null;
                    collectionItemParent = null;
                }
            }

            // hide UI when ray does not hit any object
            else
            {
                collectionItemUI.SetActive(false);
                collectionItemBG.SetActive(false);
                collectionItem = null;
                collectionItemParent = null;
            }
        }

        // hide UI on touch up or when ray no longer hits object
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            collectionItemUI.SetActive(false);
            collectionItemBG.SetActive(false);
            collectionItem = null;
            collectionItemParent = null;
        }
    }


    void FixedUpdate()
    {

    }
}
