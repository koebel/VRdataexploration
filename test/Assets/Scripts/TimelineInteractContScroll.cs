using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class TimelineInteractContScroll : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    // Timeline Interactor
    public GameObject timelineInteractor;
    private GameObject controller;
    private GameObject camrig;

    // number of Scenes
    private int size = 4;

    // Define scenes
    private string scene1 = "Pavillon1952";
    private string scene2 = "Pavillon1984";
    private string scene3 = "Pavillon2007";
    private string scene4 = "Pavillon2013";
    private string testscene = "Demo";
    // testscene is called with grip button
    // could serve for the intro/training scene
    private string currentScene;

    // materials for each scene
    private Material mat1952;
    private Material mat1984;
    private Material mat2007;
    private Material mat2013;

    private Material matActive1952;
    private Material matActive1984;
    private Material matActive2007;
    private Material matActive2013;

    private Material defaultMat;

    // child objects
    private GameObject side1;
    private GameObject side2;
    private GameObject side3;
    private GameObject side4;
    private GameObject cube;
    private GameObject top;
    private GameObject bottom;

    private bool menuActive = false;
    private bool switchSceneOK = false;
    private bool touched = false;
    public static bool timelineActive = false;

    // visible sides of cube
    private int current = 0;
    private int prev;
    private int next;

    // current rotation of cube
    private int orientation = 0;

    // Define rotation axis
    private float current_rot = -45.0f;
    private float current_euler;
    private float prev_euler;


    // x_move measures the movement on the trackpad
    // values range between -1 and 1
    private float x_move = 0.0f;
    private float x_current = 0.0f;
    private float movement;
    public float speed = 100.0f;

    // create array for materials
    public Material[,] matArray;
   

    void Start() {
        // assign parent objects of timeline interactor
        controller = timelineInteractor.transform.parent.gameObject;
        camrig = controller.transform.parent.gameObject;

        // assign child objects of timeline interactor to variables
        side1 = timelineInteractor.transform.Find("Side1").gameObject;
        side2 = timelineInteractor.transform.Find("Side2").gameObject;
        side3 = timelineInteractor.transform.Find("Side3").gameObject;
        side4 = timelineInteractor.transform.Find("Side4").gameObject;

        cube = timelineInteractor.transform.Find("Cube").gameObject;
        top = timelineInteractor.transform.Find("Top").gameObject;
        bottom = timelineInteractor.transform.Find("Bottom").gameObject;

        // load Materials
        mat1952 = (Material)Resources.Load("Timeline/Materials/1952_passiv", typeof(Material));
        mat1984 = (Material)Resources.Load("Timeline/Materials/1984_passiv", typeof(Material));
        mat2007 = (Material)Resources.Load("Timeline/Materials/2007_passiv", typeof(Material));
        mat2013 = (Material)Resources.Load("Timeline/Materials/2013_passiv", typeof(Material));

        matActive1952 = (Material)Resources.Load("Timeline/Materials/1952_aktiv", typeof(Material));
        matActive1984 = (Material)Resources.Load("Timeline/Materials/1984_aktiv", typeof(Material));
        matActive2007 = (Material)Resources.Load("Timeline/Materials/2007_aktiv", typeof(Material));
        matActive2013 = (Material)Resources.Load("Timeline/Materials/2013_aktiv", typeof(Material));

        defaultMat = (Material)Resources.Load("Timeline/Materials/DefaultMaterial", typeof(Material));

        // set color of cube, top & bottom
        defaultMat.color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        cube.GetComponent<MeshRenderer>().material = defaultMat;
        top.GetComponent<MeshRenderer>().material.color = new Color(0.25f, 0.25f, 0.25f, 1.0f);
        bottom.GetComponent<MeshRenderer>().material.color = new Color(0.15f, 0.15f, 0.15f, 1.0f);

        // create array for materials
        matArray = new Material[size, 2];

        // fill array with materials
        matArray[0, 0] = mat1952;
        matArray[0, 1] = matActive1952;
        matArray[1, 0] = mat1984;
        matArray[1, 1] = matActive1984;
        matArray[2, 0] = mat2007;
        matArray[2, 1] = matActive2007;
        matArray[3, 0] = mat2013;
        matArray[3, 1] = matActive2013;

        // initialisierung der positionsmarker
        next = current + 1;
        prev = current - 1 + size;
    }


    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    void FixedUpdate() {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (menuActive)
        {

            switch (orientation)
            {
                case 0:
                    side1.GetComponent<MeshRenderer>().material = matArray[next, 0];
                    side2.GetComponent<MeshRenderer>().material = defaultMat;
                    side3.GetComponent<MeshRenderer>().material = matArray[prev, 0];
                    side4.GetComponent<MeshRenderer>().material = matArray[current, 1];
                    break;

                case 1:
                    side1.GetComponent<MeshRenderer>().material = matArray[current, 1];
                    side2.GetComponent<MeshRenderer>().material = matArray[next, 0];
                    side3.GetComponent<MeshRenderer>().material = defaultMat;
                    side4.GetComponent<MeshRenderer>().material = matArray[prev, 0];
                    break;

                case 2:
                    side1.GetComponent<MeshRenderer>().material = matArray[prev, 0];
                    side2.GetComponent<MeshRenderer>().material = matArray[current, 1];
                    side3.GetComponent<MeshRenderer>().material = matArray[next, 0];
                    side4.GetComponent<MeshRenderer>().material = defaultMat;
                    break;

                case 3:
                    side1.GetComponent<MeshRenderer>().material = defaultMat;
                    side2.GetComponent<MeshRenderer>().material = matArray[prev, 0];
                    side3.GetComponent<MeshRenderer>().material = matArray[current, 1];
                    side4.GetComponent<MeshRenderer>().material = matArray[next, 0];
                    break;

                default:
                    Debug.Log("orientation not valid");
                    break;
            }
        }

        // show timeline interactor
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu)) 
        {
            // display Cube
            if (!menuActive) {
                timelineInteractor.SetActive(true);
                menuActive = true;
                switchSceneOK = true;
                timelineActive = true;
            }
            // hide Cube
            else{
                timelineInteractor.SetActive(false);
                menuActive = false;
                switchSceneOK = false;
                timelineActive = false;
            }
        }

        // Select Scene
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && device.GetAxis().y >= 0.0f && switchSceneOK)
        {
            timelineInteractor.SetActive(false);
            menuActive = false;
            switchSceneOK = false;
            timelineActive = false;

            // Set default Position as start position for next scene
            SetStartposition.position = SetStartposition.defaultposition;

            // switch to currently selected scene & get to default position
            switchScene(current);           
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && device.GetAxis().y < 0.0f && switchSceneOK)
        // if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger) && switchSceneOK)
        {
            timelineInteractor.SetActive(false);
            menuActive = false;
            switchSceneOK = false;
            timelineActive = false;

            // Set current position as start position for next scene
            SetStartposition.position = camrig.transform.position;
            SetStartposition.currentYear = current;

            // switch to currently selected scene & keep current position
            switchScene(current);
        }

        // Switch to Test scene
        if (device.GetPress(SteamVR_Controller.ButtonMask.Grip) && switchSceneOK)
        {
            timelineInteractor.SetActive(false);
            menuActive = false;
            switchSceneOK = false;
            timelineActive = false;

            SetStartposition.currentYear = current;
            SceneManager.LoadScene(testscene);
        }

        // Checks for Touchpad touch swipe      
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)&&menuActive)
        {
            if (!touched) {
                touched = true;
                x_move = device.GetAxis().x;
            }

            else {
                x_current = device.GetAxis().x;
                movement = x_move - x_current;
                // Set Rotation angle
                Transform transform = timelineInteractor.transform;
                timelineInteractor.transform.Rotate(0.0f, movement * speed, 0.0f);
                x_move = x_current;

                // get current position based on movement
                current_rot += movement*speed;

                // get values from 0 to 3 to get current orientation of cube
                orientation = (int)current_rot / 90;

                // korrektur divison neg zahlen
                if(current_rot > 0)
                {
                    orientation += 1;
                }
                orientation %= 4;
                if (orientation < 0)
                {
                    orientation += 4;
                }

                // get values from 0 to size -1 for current exhibition
                current = (int) current_rot / 90;

                // korrektur divison neg zahlen
                if (current_rot > 0)
                {
                    current += 1;
                }

                current %= size;
                if (current < 0) {
                    current += size;
                }
                next = (current + 1) % size;
                prev = (current - 1 + size) % size;
            }
        }

        // reset x_move to zero when touch is released
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            x_move = 0.0f;
            touched = false;
        }   
    }


    private void switchScene(int current){

        currentScene = SceneManager.GetActiveScene().name;

        // switch should have the same number of cases as value of variable "size"
        switch (current)
            {
                case 0:
                    if(currentScene != scene1) {
                        Debug.Log("switch scene to 1952");
                        SceneManager.LoadScene(scene1); 
                }
                    break;

                case 1: 
                    if (currentScene != scene2) {
                        Debug.Log("switch scene to 1984");
                        SceneManager.LoadScene(scene2);
                }
                    break;

                case 2:
                    if (currentScene != scene3) {
                        Debug.Log("switch scene to 2007");
                        SceneManager.LoadScene(scene3);
                    }
                    break;

                case 3:
                    if (currentScene != scene4) {
                        Debug.Log("switch scene to 2013");
                        SceneManager.LoadScene(scene4);
                    }
                    break;

                default:
                    Debug.Log("scene selection not valid");
                    break;
            }
    }
}
