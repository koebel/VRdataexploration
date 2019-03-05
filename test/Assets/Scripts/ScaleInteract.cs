using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


namespace ScaleInteractionSpace { 

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
        private float maxScale = 300.0f;
        private float minScale = 1.0f;
        private bool scalingActive = true;

        // Materials
        public Material standardMaterial;
        public Material selectedMaterial;

        //Marker
        private GameObject marker;
        public float markerSize = 0.05f;
        private float markerRadiusFactor = 0.265f;
        private float markerOffsetHorizontal = -0.275f;
        private float markerOffsetVertical = 0.275f;
        private float markerOffsetDepth = 0.05f;
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

        // ZoomLevels
        public GameObject rootOutlinesLevel1;
        public GameObject rootOutlinesLevel2;
        public GameObject rootOutlinesLevel3;

        private int zoomLevel1Boundary;
        private int zoomLevel2Boundary;
        //private int zoomLevel3Boundary;

        private bool zoomLevel1Active = false;
        private bool zoomLevel2Active = false;
        private bool zoomLevel3Active = false;

        public static int currentZoomLevel = -1;
        public static float currentStaticScaleFactor = -1;


            void Start() {
            //currentScaleFactor = 1f;
            currentScaleFactor = maxScale/scaleFactor;
            currentStaticScaleFactor = currentScaleFactor;
            camrig.transform.localScale = new Vector3(currentScaleFactor, currentScaleFactor, currentScaleFactor);

            //calculate zoom level boundaries
            zoomLevel1Boundary = (int) (currentScaleFactor / scaleFactor);
            zoomLevel2Boundary = (int) (zoomLevel1Boundary / scaleFactor);
            //zoomLevel3Boundary = (int) (ZoomLevel2Boundary / scaleFactor);

            // set outlines to zoomlevel
            //zoomLevel1Active = true;
            SetZoomLevel(currentScaleFactor);

            // find child objects of zoom interactor 
            top = zoomInteractor.transform.Find("zoom-top").gameObject;
            top = top.transform.Find("default").gameObject;

            bottom = zoomInteractor.transform.Find("zoom-bottom").gameObject;
            bottom = bottom.transform.Find("default").gameObject;

            //plus = zoomInteractor.transform.Find("zoom-plus").gameObject;
            plus = zoomInteractor.transform.Find("zoom-icn-up").gameObject;
            plus = plus.transform.Find("default").gameObject;

            //minus = zoomInteractor.transform.Find("zoom-minus").gameObject;
            minus = zoomInteractor.transform.Find("zoom-icn-down").gameObject;
            minus = minus.transform.Find("default").gameObject;

            top.GetComponent<MeshRenderer>().material = standardMaterial;
            bottom.GetComponent<MeshRenderer>().material = selectedMaterial;

            // create Marker that indicates input position on controller
            marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Collider c = marker.GetComponent<Collider>();
            c.enabled = false;
            marker.SetActive(false);

            // set position of marker
            marker.transform.parent = zoomInteractor.transform;
            marker.transform.localScale = new Vector3(markerSize, markerSize, markerSize);
            marker.transform.localPosition = new Vector3(markerOffsetHorizontal, markerOffsetDepth, markerOffsetVertical);
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

        
            // set position of pointer on zoom controller on Touchpad Touch
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {

                if (!touched)
                {
                    // set position before activating marker
                    marker.transform.localPosition = new Vector3(device.GetAxis().x * markerRadiusFactor * (-1) + markerOffsetHorizontal, markerOffsetDepth, (device.GetAxis().y * markerRadiusFactor * (-1) + markerOffsetVertical));
                    marker.SetActive(true);
                    touched = true;
                }
                else
                {
                    marker.transform.localPosition = new Vector3(device.GetAxis().x * markerRadiusFactor * (-1) + markerOffsetHorizontal, markerOffsetDepth, (device.GetAxis().y * markerRadiusFactor * (-1) + markerOffsetVertical));
                }

                // set color of buttons when active
                // plus/up
                if (device.GetAxis().y > 0)
                {
                    bottom.GetComponent<MeshRenderer>().material = standardMaterial;

                    if (currentScaleFactor < maxScale)
                    {
                        top.GetComponent<MeshRenderer>().material = selectedMaterial;
                    }
                    else {
                        top.GetComponent<MeshRenderer>().material = standardMaterial;
                    }
                }
                // minus/down
                else if (device.GetAxis().y < 0)
                {
                    top.GetComponent<MeshRenderer>().material = standardMaterial;

                    if (currentScaleFactor > minScale)
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
                if (device.GetAxis().y > 0 && currentScaleFactor < maxScale)
                {
                    currentScaleFactor *= scaleFactor;
                    currentStaticScaleFactor = currentScaleFactor;

                    //Transform transform = cube.transform;
                    camrig.transform.localScale = new Vector3(currentScaleFactor, currentScaleFactor, currentScaleFactor);

                    //set ZoomLevels
                    SetZoomLevel(currentScaleFactor);
                }

                // scale down (zoom in)
                else if (device.GetAxis().y < 0 && currentScaleFactor > minScale)
                {
                    currentScaleFactor /= scaleFactor;
                    currentStaticScaleFactor = currentScaleFactor;
                    //Debug.Log("Scalefactor " + currentScaleFactor);

                    //Transform transform = cube.transform;
                    camrig.transform.localScale = new Vector3(currentScaleFactor, currentScaleFactor, currentScaleFactor);

                    //set ZoomLevels
                    SetZoomLevel(currentScaleFactor);
                }
            }

            // hide marker and reset materials if touch is released
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                // hide marke on touch up
                marker.SetActive(false);

                // reset materials of controller elements to standard
                top.GetComponent<MeshRenderer>().material = standardMaterial;
                bottom.GetComponent<MeshRenderer>().material = standardMaterial;

                touched = false;
            }   
        }

        public void SetZoomLevel(float f) {

            //activate Zoom Level 1
            if (f > zoomLevel1Boundary)
            {
                if (!zoomLevel1Active) {
                    rootOutlinesLevel1.SetActive(true);
                    rootOutlinesLevel2.SetActive(false);
                    rootOutlinesLevel3.SetActive(false);

                    zoomLevel1Active = true;
                    zoomLevel2Active = false;
                    zoomLevel3Active = false;

                    currentZoomLevel = 1;
                }   
            }

            // activate Zoom Level 2
            else if (f > zoomLevel2Boundary)
            {
                if (!zoomLevel2Active)
                {
                    rootOutlinesLevel1.SetActive(false);
                    rootOutlinesLevel2.SetActive(true);
                    rootOutlinesLevel3.SetActive(false);

                    zoomLevel1Active = false;
                    zoomLevel2Active = true;
                    zoomLevel3Active = false;

                    currentZoomLevel = 2;
                }
            }

            // activate Zoom Level 3
            else {
                if (!zoomLevel3Active)
                {
                    rootOutlinesLevel1.SetActive(false);
                    rootOutlinesLevel2.SetActive(false);
                    rootOutlinesLevel3.SetActive(true);

                    zoomLevel1Active = false;
                    zoomLevel2Active = false;
                    zoomLevel3Active = true;

                    currentZoomLevel = 3;
                }
            }
        }

        public int getZoomLevel()
        {
                return currentZoomLevel;
        }
    }

}