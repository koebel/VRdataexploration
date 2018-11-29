using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SceneSwitcher : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    // Timeline Interactor
    public GameObject sceneSwitcher;
    public Material defaultMaterial;
    private GameObject controller;
    private GameObject camrig;

    // number of Scenes
    private int size = 4;

    // Define scenes
    private string scene1 = "Experiment1";
    private string scene2 = "Experiment2";
    private string scene3 = "Experiment3";
    private string scene4 = "Experiment4";
    private string testscene = "Experiment1";
    // testscene is called with grip button
    // could serve for the intro/training scene
    private string currentScene;

    // materials for each scene
    private Material matExperiment1;
    private Material matExperiment2;
    private Material matExperiment3;
    private Material matExperiment4;

    private Material matActiveExperiment1;
    private Material matActiveExperiment2;
    private Material matActiveExperiment3;
    private Material matActiveExperiment4;

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
    public static bool switcherActive = false;

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
        controller = sceneSwitcher.transform.parent.gameObject;
        camrig = controller.transform.parent.gameObject;

        // assign child objects of timeline interactor to variables
        side1 = sceneSwitcher.transform.Find("Side1").gameObject;
        side2 = sceneSwitcher.transform.Find("Side2").gameObject;
        side3 = sceneSwitcher.transform.Find("Side3").gameObject;
        side4 = sceneSwitcher.transform.Find("Side4").gameObject;

        cube = sceneSwitcher.transform.Find("Cube").gameObject;
        top = sceneSwitcher.transform.Find("Top").gameObject;
        bottom = sceneSwitcher.transform.Find("Bottom").gameObject;

        // load Materials
        matExperiment1 = (Material)Resources.Load("Experiments/Materials/experiment1_brighter", typeof(Material));
        matExperiment2 = (Material)Resources.Load("Experiments/Materials/experiment2_brighter", typeof(Material));
        //matExperiment2 = defaultMaterial;
        matExperiment3 = (Material)Resources.Load("Experiments/Materials/experiment3_brighter", typeof(Material));
        matExperiment4 = (Material)Resources.Load("Experiments/Materials/experiment4_brighter", typeof(Material));

        matActiveExperiment1 = (Material)Resources.Load("Experiments/Materials/experiment1", typeof(Material));
        matActiveExperiment2 = (Material)Resources.Load("Experiments/Materials/experiment2", typeof(Material));
        //matActiveExperiment2 = defaultMaterial;
        matActiveExperiment3 = (Material)Resources.Load("Experiments/Materials/experiment3", typeof(Material));
        matActiveExperiment4 = (Material)Resources.Load("Experiments/Materials/experiment4", typeof(Material));

        //defaultMat = (Material)Resources.Load("Timeline/Materials/BlackNoReflection", typeof(Material));
        defaultMat = defaultMaterial;

        // set color of cube, top & bottom
        // defaultMat.color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        cube.GetComponent<MeshRenderer>().material = defaultMat;
        //top.GetComponent<MeshRenderer>().material.color = new Color(0.25f, 0.25f, 0.25f, 1.0f);
        //bottom.GetComponent<MeshRenderer>().material.color = new Color(0.15f, 0.15f, 0.15f, 1.0f);
        top.GetComponent<MeshRenderer>().material = defaultMat;
        bottom.GetComponent<MeshRenderer>().material = defaultMat;

        // create array for materials
        matArray = new Material[size, 2];

        // fill array with materials
        matArray[0, 0] = matExperiment1;
        matArray[0, 1] = matActiveExperiment1;
        matArray[1, 0] = matExperiment2;
        matArray[1, 1] = matActiveExperiment2;
        matArray[2, 0] = matExperiment3;
        matArray[2, 1] = matActiveExperiment3;
        matArray[3, 0] = matExperiment4;
        matArray[3, 1] = matActiveExperiment4;

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
                sceneSwitcher.SetActive(true);
                menuActive = true;
                switchSceneOK = true;
                switcherActive = true;
            }
            // hide Cube
            else{
                sceneSwitcher.SetActive(false);
                menuActive = false;
                switchSceneOK = false;
                switcherActive = false;
            }
        }

        // Select Scene
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && switchSceneOK)
        {
            sceneSwitcher.SetActive(false);
            menuActive = false;
            switchSceneOK = false;
            switcherActive = false;

            // Set current position as start position for next scene
            SetStartposition.position = camrig.transform.position;
            SetStartposition.currentYear = current;

            // switch to currently selected scene & get to default position
            switchScene(current);
        }
        /*
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && device.GetAxis().y >= 0.0f && switchSceneOK)
        {
            sceneSwitcher.SetActive(false);
            menuActive = false;
            switchSceneOK = false;
            switcherActive = false;

            // Set default Position as start position for next scene
            SetStartposition.position = SetStartposition.defaultposition;

            // switch to currently selected scene & get to default position
            switchScene(current);           
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && device.GetAxis().y < 0.0f && switchSceneOK)
        {
            sceneSwitcher.SetActive(false);
            menuActive = false;
            switchSceneOK = false;
            switcherActive = false;

            // Set current position as start position for next scene
            SetStartposition.position = camrig.transform.position;
            SetStartposition.currentYear = current;

            // switch to currently selected scene & keep current position
            switchScene(current);
        }
        */

        /*
        // Switch to Test scene
        if (device.GetPress(SteamVR_Controller.ButtonMask.Grip) && switchSceneOK)
        {
            sceneSwitcher.SetActive(false);
            menuActive = false;
            switchSceneOK = false;
            switcherActive = false;

            SetStartposition.currentYear = current;
            SceneManager.LoadScene(testscene);
        }
        */

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
                Transform transform = sceneSwitcher.transform;
                sceneSwitcher.transform.Rotate(0.0f, movement * speed, 0.0f);
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
                        Debug.Log("switch scene to " + scene1);
                        SceneManager.LoadScene(scene1); 
                }
                    break;

                case 1: 
                    if (currentScene != scene2) {
                        Debug.Log("switch scene to " + scene2);
                        SceneManager.LoadScene(scene2);
                }
                    break;

                case 2:
                    if (currentScene != scene3) {
                        Debug.Log("switch scene to " + scene3);
                        SceneManager.LoadScene(scene3);
                    }
                    break;

                case 3:
                    if (currentScene != scene4) {
                        Debug.Log("switch scene to " + scene4);
                        SceneManager.LoadScene(scene4);
                    }
                    break;

                default:
                    Debug.Log("scene selection not valid");
                    break;
            }
    }
}
