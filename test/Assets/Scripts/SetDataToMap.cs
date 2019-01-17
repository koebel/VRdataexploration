using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CollectionDataHandlingSpace;

public class SetDataToMap : MonoBehaviour {

    // attributes
    public GameObject worldVolumes;
    //private List<GameObject> allCountries;
    private GameObject temp;

    // color schema
    public Material matHighest;
    public Material matHigh;
    public Material matMedium;
    public Material matLow;
    public Material matLowest;

    private int countriesTotal;
    private int itemsTotal;
    private int maxValue;

	// Use this for initialization
	void Start () {
        Debug.Log("setDataToMap");

        countriesTotal = CollectionDataHandling.CollectionData.countryStats.Count;
        itemsTotal = CollectionDataHandling.CollectionData.allItems.Count;
        maxValue = CollectionDataHandling.CollectionData.getHighestDictionaryValue();

        // allCountries = new List<GameObject>();

        foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStats)
        {
            temp = worldVolumes.transform.Find(p.Key).gameObject;
            temp = temp.transform.Find("default").gameObject;
            // todo set intervals
            if (p.Value == maxValue && temp.GetComponent<MeshRenderer>() != null) {
                temp.GetComponent<MeshRenderer>().material = matHighest;
                temp.transform.localScale = new Vector3(1, 5, 1);
            }

            if (p.Value == maxValue/2 && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matMedium;
            }
        }
    }

    // Use this for initialization
    void setDataToMap()
    {
        Debug.Log("setDataToMap");

        countriesTotal = CollectionDataHandling.CollectionData.countryStats.Count;
        itemsTotal = CollectionDataHandling.CollectionData.allItems.Count;
        maxValue = CollectionDataHandling.CollectionData.getHighestDictionaryValue();

        // allCountries = new List<GameObject>();

        foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStats)
        {
            temp = worldVolumes.transform.Find(p.Key).gameObject;
            temp = temp.transform.Find("default").gameObject;
            // todo set intervals
            if (p.Value == maxValue && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matHighest;
                temp.transform.localScale = new Vector3(1, 5, 1);
            }

            if (p.Value == maxValue / 2 && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matMedium;
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
