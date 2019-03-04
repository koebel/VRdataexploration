using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZoomLevels : MonoBehaviour {

    public GameObject rootOutlinesLevel1;
    public GameObject rootOutlinesLevel2;
    public GameObject rootOutlinesLevel3;



    public float zoomFactorItemsLevel1 = 3.0f;
    public float zoomFactorItemsLevel2 = 3.0f;
    public float zoomFactorItemsLevel3 = 3.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Scalefactor: " + ScaleInteractionSpace.ScaleInteract.currentScaleFactor);
	}
}
