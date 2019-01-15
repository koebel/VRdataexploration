using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialOfChildren : MonoBehaviour {

    public Material mat;
    public GameObject root;
    private GameObject temp;

    private int numberOfChildren;
    private GameObject[] allChildren;

    // Use this for initialization
    void Start () {

        // count number of children of root object
        numberOfChildren = root.transform.childCount;
        Debug.Log("Children: " + numberOfChildren);

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
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
