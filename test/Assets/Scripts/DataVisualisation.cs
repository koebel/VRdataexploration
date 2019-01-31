using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CollectionDataHandlingSpace;

public class DataVisualisation : MonoBehaviour {

    public Material matHighest;
    public Material matHigh;
    public Material matMedium;
    public Material matLow;
    public Material matLowest;

    public Material matDefault;

    public float elevation = 10.0f;

    public float scaleFactor = 2.0f;

    public GameObject rootCountryVolumes;
    public GameObject rootCollectionItems;
    private GameObject temp;

    private int maxValue;

    public void resetVisualisation() {

        // reset country volumes to default
        foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStats)
        {
            temp = rootCountryVolumes.transform.Find(p.Key).gameObject;
            temp = temp.transform.Find("default").gameObject;
            
            // reset material & size
            if (temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matDefault;
                temp.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        // reset collection items to default
        foreach (CollectionDataHandling.CollectionItem item in CollectionDataHandling.CollectionData.allItems)
        {
            temp = rootCollectionItems.transform.Find(item.objectRef).gameObject;
            // reset size
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void applySelection() {
        maxValue = CollectionDataHandling.CollectionData.getHighestDictionaryValue();

        // apply to country volumes
        foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStatsSelection)
        {
            temp = rootCountryVolumes.transform.Find(p.Key).gameObject;
            temp = temp.transform.Find("default").gameObject;
            // todo set intervals
            if (p.Value == maxValue && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matHighest;
                temp.transform.localScale = new Vector3(1, elevation, 1);
            }

            if (p.Value == maxValue / 2 && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matMedium;
                temp.transform.localScale = new Vector3(1, elevation/2, 1);
            }
        }

        // apply selection to items
        foreach (CollectionDataHandling.CollectionItem item in CollectionDataHandling.CollectionData.allItems)
        {
            temp = rootCollectionItems.transform.Find(item.objectRef).gameObject;
            temp.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
