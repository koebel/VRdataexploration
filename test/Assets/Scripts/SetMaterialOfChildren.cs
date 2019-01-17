using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CollectionDataHandlingSpace;

public class SetMaterialOfChildren : MonoBehaviour {

    public Material mat;

    public Material matHighest;
    public Material matHigh;
    public Material matMedium;
    public Material matLow;
    public Material matLowest;

    public GameObject root;
    private GameObject temp;

    private int numberOfChildren;
    private GameObject[] allChildren;

    private int maxValue;

    // Use this for initialization
    void Start () {

        // count number of children of root object
        numberOfChildren = root.transform.childCount;
        //Debug.Log("Children: " + numberOfChildren);

        // create array for children
        allChildren = new GameObject[numberOfChildren];

        //Debug.Log("array: " + allChildren.Length);

        // fill array with items
        for (int i = 0; i < allChildren.Length; i++)
        {
            temp = root.transform.GetChild(i).gameObject;
            //Debug.Log(temp.name);
            temp = temp.transform.Find("default").gameObject;
            //Debug.Log(temp.name);
            // check if item has mesh
            if (temp.GetComponent<MeshRenderer>() != null) {
                temp.GetComponent<MeshRenderer>().material = mat;
                //Debug.Log("set mat");
            }
            allChildren[i] = temp;
        }

        // set data to map
        maxValue = CollectionDataHandling.CollectionData.getHighestDictionaryValue();

        foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStats)
        {
            temp = root.transform.Find(p.Key).gameObject;
            temp = temp.transform.Find("default").gameObject;
            // todo set intervals
            if (p.Value == maxValue && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matHighest;
                temp.transform.localScale = new Vector3(1, 10, 1);
            }

            if (p.Value == maxValue / 2 && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matMedium;
                temp.transform.localScale = new Vector3(1, 5, 1);
            }
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
