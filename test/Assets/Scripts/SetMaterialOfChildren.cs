using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CollectionDataHandlingSpace;

public class SetMaterialOfChildren : MonoBehaviour {

    public Material mat;
    public GameObject root;
    private GameObject temp;

    private int numberOfChildren;
    private GameObject[] allChildren;

    // Use this for initialization
    void Start () {
        // just for checking the loading order of scripts
        Debug.Log("Set Material of " + root.name);

        // count number of children of root object
        numberOfChildren = root.transform.childCount;

        // create array for children
        allChildren = new GameObject[numberOfChildren];

        // fill array with items
        for (int i = 0; i < allChildren.Length; i++)
        {
            temp = root.transform.GetChild(i).gameObject;
            temp = temp.transform.Find("default").gameObject;

            // check if item has mesh
            if (temp.GetComponent<MeshRenderer>() != null) {
                temp.GetComponent<MeshRenderer>().material = mat;
            }
            allChildren[i] = temp;
        }
    }
	

	// Update is called once per frame
	void Update () {
		
	}

}
