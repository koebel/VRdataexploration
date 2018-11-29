using UnityEngine;
using System.Collections;

public class SituativeTooltips : MonoBehaviour {

    public GameObject tooltipsTime;
    public GameObject tooltipsTimeActive;
    public GameObject tooltipsGuide;
    public GameObject tooltipsGuideActive;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // Ausstellungsguide active
        if (DisplayMetaInformation.guideActive)
        {
            tooltipsGuide.SetActive(false);
            tooltipsGuideActive.SetActive(true);
            Debug.Log("Ausstellungguide active");
        }
        // Ausstellungsguide not active
        else {
            Debug.Log("Ausstellungguide not active");
            tooltipsGuide.SetActive(true);
            tooltipsGuideActive.SetActive(false);
        }

        // Timeline Interactor active
        if (TimelineInteractContScroll.timelineActive)
        {
            Debug.Log("Zeitreise active");
            tooltipsTime.SetActive(false);
            tooltipsTimeActive.SetActive(true);
        }
        // Timeline Interactor not active
        else
        {
            Debug.Log("Zeitreise not active");
            tooltipsTime.SetActive(true);
            tooltipsTimeActive.SetActive(false);
        }
    }
}