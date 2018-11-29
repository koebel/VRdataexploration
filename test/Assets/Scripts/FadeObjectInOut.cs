/*
	FadeObjectInOut.cs
 	Hayden Scott-Baron (Dock) - http://starfruitgames.com
 	6 Dec 2012 
 
	This allows you to easily fade an object and its children. 
	If an object is already partially faded it will continue from there. 
	If you choose a different speed, it will use the new speed. 
 
	NOTE: Requires materials with a shader that allows transparency through color.  

    Updated by Kathrin Koebel on 28 June 2016 to fit the needs 
    of Points of Interest Scenario of the Biennale 4D project
*/

using UnityEngine;
using System.Collections;

public class FadeObjectInOut : MonoBehaviour
{

    // publically editable speed
    public float fadeDelay = 0.0f;
    public float fadeTime = 0.5f;
    public bool visibleOnStart = false;
    private bool logInitialFadeSequence = false;
    public float distance = 1.0f;

    private float current_dist;
    private bool outside = true;

    public Renderer rend;
    public GameObject POI;


    // store colours
    private Color[] colors;

    // allow automatic fading on the start of the scene
    IEnumerator Start()
    {
        //yield return null; 
        yield return new WaitForSeconds(fadeDelay);

        if (visibleOnStart)
        {
            logInitialFadeSequence = true;
            FadeIn();
        }

        else
        {
            FadeOut(fadeTime);
        }

        
    }

    void Awake()
    {
        FadeOut(0);
    }

    void Update()
    {
        // display billboard
        current_dist = Vector3.Distance(Camera.main.transform.position, POI.transform.position);
        //Debug.Log(current_dist);

        // user enters into the defined range
        if (current_dist <= distance && outside)
        {
            outside = false;
            FadeIn();
        }
        // user leaves the defined range
        if (!outside && current_dist > distance)
        {
            outside = true;
            FadeOut();
        }
    }


    // check the alpha value of most opaque object
    float MaxAlpha()
    {
        float maxAlpha = 0.0f;
        Renderer[] rendererObjects = GetComponentsInChildren<Renderer>();
        foreach (Renderer item in rendererObjects)
        {
            maxAlpha = Mathf.Max(maxAlpha, item.material.color.a);
        }
        return maxAlpha;
    }

    // fade sequence
    IEnumerator FadeSequence(float fadingOutTime)
    {
        // log fading direction, then precalculate fading speed as a multiplier
        bool fadingOut = (fadingOutTime < 0.0f);
        float fadingOutSpeed = 1.0f / fadingOutTime;

        // grab all child objects
        Renderer[] rendererObjects = GetComponentsInChildren<Renderer>();
        if (colors == null)
        {
            //create a cache of colors if necessary
            colors = new Color[rendererObjects.Length];

            // store the original colours for all child objects
            for (int i = 0; i < rendererObjects.Length; i++)
            {
                colors[i] = rendererObjects[i].material.color;
            }
        }

        // make all objects visible
        for (int i = 0; i < rendererObjects.Length; i++)
        {
            rendererObjects[i].enabled = true;
        }

        // get current max alpha
        float alphaValue = MaxAlpha();

        // This is a special case for objects that are set to fade in on start. 
        // it will treat them as alpha 0, despite them not being so. 
        if (logInitialFadeSequence && !fadingOut)
        {
            alphaValue = 0.0f;
            logInitialFadeSequence = false;
        }

        // iterate to change alpha value 
        while ((alphaValue >= 0.0f && fadingOut) || (alphaValue <= 1.0f && !fadingOut))
        {
            alphaValue += Time.deltaTime * fadingOutSpeed;

            for (int i = 0; i < rendererObjects.Length; i++)
            {
                Color newColor = (colors != null ? colors[i] : rendererObjects[i].material.color);
                newColor.a = Mathf.Min(newColor.a, alphaValue);
                newColor.a = Mathf.Clamp(newColor.a, 0.0f, 1.0f);
                rendererObjects[i].material.color = newColor;
            }

            yield return null;
        }

        // turn objects off after fading out
        if (fadingOut)
        {
            for (int i = 0; i < rendererObjects.Length; i++)
            {
                rendererObjects[i].enabled = false;
            }
        }


        Debug.Log("fade sequence end : " + fadingOut);

    }


    void FadeIn()
    {
        FadeIn(fadeTime);
    }

    void FadeOut()
    {
        FadeOut(fadeTime);
    }

    void FadeIn(float newFadeTime)
    {
        StopAllCoroutines();
        StartCoroutine("FadeSequence", newFadeTime);
    }

    void FadeOut(float newFadeTime)
    {
        StopAllCoroutines();
        StartCoroutine("FadeSequence", -newFadeTime);
    }  


}