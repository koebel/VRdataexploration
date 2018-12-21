using UnityEngine.UI;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class DisplayItemInfo : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    private GameObject collectionItem;
    private GameObject collectionItemParent;
    private GameObject collectionItemUI;
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
                // retrieve metadata from hit object
                hitObject = hit.collider.gameObject;

                if (hitObject.tag == collectionItemTag)
                {
                    Debug.Log("hit collection item");
                    collectionItem = hitObject;
                    Debug.Log(collectionItem.name);

                    // set collection item 
                    //collectionItemParent = collectionItem.transform.parent;
                    //collectionUI = collectionItemParent.transform.Find("UI").gameObject;
                }


                if (hit.collider.gameObject.tag == collectionItemTag)
                {
                    //collectionUI.SetActive(true);
                }
                
                // hide marker when ray hits object of different type
                else
                {
                    //collectionUI.SetActive(false);
                }
            }

            // hide marker when ray does not hit any object
            else
            {
                //collectionUI.SetActive(false);
            }
        }

        // hide text on touch up or when ray no longer hits object
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            //collectionUI.SetActive(false);
        }
    }


    void FixedUpdate()
    {

    }
}
