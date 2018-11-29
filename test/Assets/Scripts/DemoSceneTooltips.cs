using UnityEngine;
using System.Collections;

public class DemoSceneTooltips : MonoBehaviour {

	// set status for timeline interactor and guide in demo scene
	void Start () {
        TimelineInteractContScroll.timelineActive = false;
        DisplayMetaInformation.guideActive = false;
    }
}
