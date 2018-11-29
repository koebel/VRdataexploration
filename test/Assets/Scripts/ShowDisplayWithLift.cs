using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
	function to show display when user enters a given radius of designated marker object
    display can either be the display item itself or a parent object that contains multiple nested items
    created on 19th july 2018 by k2

    IMPORTANT:
 	rendering mode of the shader of the to "display" assigned game object shout be set to "Transparent" (Legacy Shaders/Transparent/Diffuse) in order to be able to manipulate the alpha value
*/

public class ShowDisplayWithLift : MonoBehaviour
{
    public GameObject display;
    public bool isVisibleAtStart = false;
    public GameObject collider;
    public float radius = 2.0f;
    public float speed = 1.0f;

    public bool hasMarker = true;
    public GameObject marker;
    public Material markerMaterial;
    public Material markerMaterialActive;
    public Text markerText;
    public string year;

    public GameObject platform;
    public float height = 1.0f;
    public float liftSpeedFactor = 3.0f;

    public GameObject player;

    private Vector3 currentPos;
    private Vector3 prevPos;
    private float movement;

    private Canvas[] markerCanvas;
    private Canvas markerCanvasUpper;
    private Canvas markerCanvasLower;
    private Canvas UiCanvas;

    private Transform[] displayComponents;

    private float distance;
    private bool isOutside = true;
    private float alphaHide = 0f;
    private float alphaShow = 1f;

    private bool teleportedOut = false;
    private Vector3 platformInitialPosition;
    private Vector3 markerCanvasInitialPosition;
    private Vector3 playerInitialPosition;
    private Vector3 playerCurrentPosition;

    // set update frequence for transitions (e.g. fade)
    private float frequence = 90;
    private float updateFrequence;

    private Color displayCol;
    private Color markerMaterialCol;
    private Color markerMaterialActiveCol;
    private Color currentColor;
    public Color textCol;
    public Color textActiveCol;
    private Color currentTextColor;
    private float textOpacity = 0.8f;

    // Use this for initialization
    void Start()
    {
        // calculate update frequence
        updateFrequence = 1 / frequence;

        // get all child objects of the selected display object, including self
        displayComponents = display.GetComponentsInChildren<Transform>();

        // get all canvases
        markerCanvas = platform.GetComponentsInChildren<Canvas>();
        UiCanvas = markerCanvas[0];
        markerCanvasUpper = markerCanvas[1];      
        markerCanvasLower = markerCanvas[2];

        //UiCanvas.GetComponent<CanvasRenderer>().cull = true;

        markerCanvasInitialPosition = markerCanvasLower.transform.position;
        platformInitialPosition = platform.transform.position;
        currentPos = player.transform.position;
        prevPos = player.transform.position;
        playerInitialPosition = player.transform.position;

        /*
        foreach (Transform comp in displayComponents)
        {
            if (comp.GetComponent<MeshFilter>() != null)
            {
                Debug.Log(comp.name);
            }
        }
        */


        // set alpha of all display components to given visibility parameter
        foreach (Transform comp in displayComponents)
        {
            // check if component has a mesh
            if (comp.GetComponent<MeshFilter>() != null)
            {
                displayCol = comp.GetComponent<Renderer>().material.color;
                if (isVisibleAtStart)
                {
                    displayCol.a = alphaShow;
                }
                else
                {
                    displayCol.a = alphaHide;
                }
                comp.GetComponent<Renderer>().material.color = displayCol;
            }
        }

        if (hasMarker) {
            // define colors
            markerMaterialCol = markerMaterial.color;
            markerMaterialActiveCol = markerMaterialActive.color;

            textCol.a = textOpacity;
            textActiveCol.a = textOpacity;

            // set marker material
            if (isOutside)
            {
                marker.GetComponent<Renderer>().material = markerMaterial;
            }
            else
            {
                marker.GetComponent<Renderer>().material = markerMaterialActive;
            }

            // set marker text
            markerText.text = year;
            markerText.material.color = textCol;
        }

        // instanciate distance --> just for unity :-)
        distance = 1000f;
    }


    // Update is called once per frame
    void Update()
    {
        // show display 
        distance = Vector3.Distance(Camera.main.transform.position, collider.transform.position);
        //Debug.Log("Distance: " + distance);

        // detect teleporation
        movement = Vector3.Distance(currentPos, prevPos);
        prevPos = currentPos;
        currentPos = player.transform.position;
        
        if (!isOutside && movement > 0.2f) {
            //Debug.Log("teleportion detected");
            teleportedOut = true;
        }

        if (isOutside && distance < radius)
        {
            isOutside = false;
            StopAllCoroutines();

            // check if display is just a parent object or the actual display
            if (display.GetComponent<MeshFilter>() != null)
            {
                StartCoroutine(FadeIn(speed, alphaShow));
            }
            else
            {
                foreach (Transform comp in displayComponents)
                {
                    // check if component has a mesh
                    if (comp.GetComponent<MeshFilter>() != null)
                    {
                        StartCoroutine(FadeComponentIn(comp, speed, alphaShow));
                    }
                }
            }

            if (hasMarker) {
                // change color of marker object
                StartCoroutine(LerpColor(marker, markerMaterialActiveCol, speed));
                StartCoroutine(LerpText(textActiveCol, speed));
            }

            // lift platform up
            playerInitialPosition = player.transform.position;
            StartCoroutine(Lift(speed*liftSpeedFactor, height, true));
        }

        if (!isOutside && distance > radius)
        {
            isOutside = true;
            StopAllCoroutines();

            // check if display is just a parent object or the actual display
            if (display.GetComponent<MeshFilter>() != null)
            {
                StartCoroutine(FadeOut(speed, alphaHide));
            }
            else
            {
                foreach (Transform comp in displayComponents)
                {
                    // check if component has a mesh
                    if (comp.GetComponent<MeshFilter>() != null)
                    {
                        StartCoroutine(FadeComponentOut(comp, speed, alphaHide));
                    }
                }
            }

            if (hasMarker) {
                // change color of marker object
                StartCoroutine(LerpColor(marker, markerMaterialCol, speed));
                StartCoroutine(LerpText(textCol, speed));
            }

            // lift platform down

            //if (teleportOut)
            if (true)
            {
                platform.transform.position = platformInitialPosition;
                markerCanvasLower.transform.position = markerCanvasInitialPosition;
                playerCurrentPosition = player.transform.position;
                player.transform.position = new Vector3(playerCurrentPosition.x, playerInitialPosition.y, playerCurrentPosition.z);
                teleportedOut = false;
            }
            else {
                StartCoroutine(Lift(speed * liftSpeedFactor * 1.25f, height, false));
            }
        }
    }


    IEnumerator FadeIn(float fadeTime, float targetOpacity)
    {
        Debug.Log("FadeIn-Coroutine started");

        // get current color of material
        Color c = display.GetComponent<Renderer>().material.color;
        // calculate difference between current opacity and target opacity
        float alphaSpectrum = Mathf.Abs(targetOpacity - c.a);
        // calculate incrementation steps
        float alphaIncr = alphaSpectrum / (fadeTime * frequence);

        // fade in
        for (float alpha = c.a; alpha <= targetOpacity; alpha += alphaIncr)
        {
            c.a += alphaIncr;
            display.GetComponent<Renderer>().material.color = c;
            yield return new WaitForSeconds(updateFrequence);
        }
    }


    IEnumerator FadeComponentIn(Transform comp, float fadeTime, float targetOpacity)
    {
        // get current color of material
        Color c = comp.GetComponent<Renderer>().material.color;
        // calculate difference between current opacity and target opacity
        float alphaSpectrum = Mathf.Abs(targetOpacity - c.a);
        // calculate incrementation steps
        float alphaIncr = alphaSpectrum / (fadeTime * frequence);

        // fade in
        for (float alpha = c.a; alpha <= targetOpacity; alpha += alphaIncr)
        {
            c.a += alphaIncr;
            comp.GetComponent<Renderer>().material.color = c;
            yield return new WaitForSeconds(updateFrequence);
        }
    }


    IEnumerator FadeOut(float fadeTime, float targetOpacity)
    {
        Debug.Log("FadeOut-Coroutine started");

        // get current color of material
        Color c = display.GetComponent<Renderer>().material.color;
        // calculate difference between current opacity and target opacity
        float alphaSpectrum = Mathf.Abs(targetOpacity - c.a);
        // calculate incrementation steps
        float alphaDecr = alphaSpectrum / (fadeTime * frequence);

        // fade out
        for (float alpha = c.a; alpha >= targetOpacity; alpha -= alphaDecr)
        {
            c.a -= alphaDecr;
            display.GetComponent<Renderer>().material.color = c;
            yield return new WaitForSeconds(updateFrequence);
        }
    }


    IEnumerator FadeComponentOut(Transform comp, float fadeTime, float targetOpacity)
    {
        Debug.Log("FadeOut-Coroutine started");

        // get current color of material
        Color c = comp.GetComponent<Renderer>().material.color;
        // calculate difference between current opacity and target opacity
        float alphaSpectrum = Mathf.Abs(targetOpacity - c.a);
        // calculate incrementation steps
        float alphaDecr = alphaSpectrum / (fadeTime * frequence);

        // fade out
        for (float alpha = c.a; alpha >= targetOpacity; alpha -= alphaDecr)
        {
            c.a -= alphaDecr;
            comp.GetComponent<Renderer>().material.color = c;
            yield return new WaitForSeconds(updateFrequence);
        }
    }


    IEnumerator LerpColor(GameObject obj, Color targetColor, float time)
    {
        Debug.Log("LerpColor-Coroutine started");

        // get current color of material
        Color c = obj.GetComponent<Renderer>().material.color;

        float progress = 0;
        float increment = 1 / (time * frequence);
        while (progress < 1)
        {
            currentColor = Color.Lerp(c, targetColor, progress);
            obj.GetComponent<Renderer>().material.color = currentColor;
            progress += increment;
            yield return new WaitForSeconds(updateFrequence);
        }
    }


    IEnumerator LerpText(Color targetColor, float time)
    {
        Debug.Log("LerpText-Coroutine started");

        // get current color of material
        Color c = markerText.material.color;

        float progress = 0;
        float increment = 1 / (time * frequence);
        while (progress < 1)
        {
            currentTextColor = Color.Lerp(c, targetColor, progress);
            markerText.material.color = currentTextColor;
            progress += increment;
            yield return new WaitForSeconds(updateFrequence);
        }
    }


    IEnumerator Lift(float fadeTime, float height, bool liftUp)
    {
        // get current height of platform
        float currentHeight = platform.transform.position.y;

        // calculate lift distance
        float targetHeight;

        if (liftUp)
        {
            targetHeight = platformInitialPosition.y + height;
        }
        else
        {
            targetHeight = platformInitialPosition.y;
        }

        float liftDistance = Mathf.Abs(targetHeight - currentHeight);

        // calculate incrementation steps
        float incr = liftDistance / (fadeTime * frequence);

        // define movement vector
        Vector3 liftIncr;
        
        if (liftUp) {
            liftIncr = new Vector3(0, incr, 0);
        }
        else {
            liftIncr = new Vector3(0, -incr, 0);
        }

        // lift platform & player
        for (float i = 0.0f; i <= liftDistance; i += incr)
        {
            platform.transform.position += liftIncr;
            markerCanvasLower.transform.position -= liftIncr;
            player.transform.position += liftIncr;
            yield return new WaitForSeconds(updateFrequence);
        }
    }
}
