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
    public float defaultHeight = 1.0f;

    public float scaleFactor = 2.0f;
    private float collectionItemDepth = 0.02f;

    private float collectionItemHeight = 1.1f;
    private float countryHeight;
    private float outlineHeight = 0.05f;

    public GameObject rootCountryVolumes;
    public GameObject rootCountryOutlines;
    public GameObject rootCollectionItems;
    private GameObject temp;

    private float tempXPosition;
    private float tempYPosition;
    private float tempZPosition;

    private int maxValue;
    private int countryValue;

    private string outlineKey;


    // Use this for initialization
    void Start()
    {
        // set materials

        applyDataVisualisation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void resetDataVisualisation() {

        // reset country volumes to default
        foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStats)
        {
            // reset Country Volume
            //Debug.Log(p.Key);
            if (rootCountryVolumes.transform.Find(p.Key).gameObject != null) {
                temp = rootCountryVolumes.transform.Find(p.Key).gameObject;
                temp = temp.transform.Find("default").gameObject;
            
                // reset material & size
                if (temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matDefault;
                    temp.transform.localScale = new Vector3(1, defaultHeight, 1);
                }
            }
            else
            {
                Debug.Log("Key not found: " + p.Key);
            }

            // reset Country Outline
            outlineKey = p.Key + "_outline";
            //Debug.Log(outlineKey);
            if (rootCountryOutlines.transform.Find(outlineKey).gameObject != null)
            {
                temp = rootCountryOutlines.transform.Find(outlineKey).gameObject;
                //temp = temp.transform.Find("default").gameObject;

                // reset size
                temp.transform.localPosition = new Vector3(0, 0, 0);
            }
            else
            {
                Debug.Log("Key not found: " + outlineKey);
            }
        }

        // reset collection items to default
        foreach (CollectionDataHandling.CollectionItem item in CollectionDataHandling.CollectionData.allItems)
        {
            temp = rootCollectionItems.transform.Find(item.objectRef).gameObject;
            // reset size
            temp.transform.localScale = new Vector3(1, 1, collectionItemDepth);

            // resize height
            tempXPosition = temp.transform.position.x;
            tempYPosition = temp.transform.position.y;
            tempZPosition = temp.transform.position.z;
            temp.transform.position = new Vector3(tempXPosition, collectionItemHeight, tempZPosition);
        }
    }

    public void applyDataVisualisation()
    {
        maxValue = CollectionDataHandling.CollectionData.getHighestDictionaryValue();

        // apply to country volumes
        foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStats)
        {
            //set country volume height
            //Debug.Log(p.Key);
            if (rootCountryVolumes.transform.Find(p.Key).gameObject != null)
            {
                temp = rootCountryVolumes.transform.Find(p.Key).gameObject;
                temp = temp.transform.Find("default").gameObject;

                // set color intervals & elevation
                if (p.Value > maxValue * 0.9f && temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matHighest;
                    temp.transform.localScale = new Vector3(1, defaultHeight+elevation, 1);
                }

                else if (p.Value > maxValue * 0.7f && temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matHigh;
                    temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.8f, 1);
                }

                else if (p.Value > maxValue * 0.5f && temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matMedium;
                    temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.6f, 1);
                }

                else if (p.Value > maxValue * 0.3f && temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matLow;
                    temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.4f, 1);
                }

                else if (p.Value > 0 && temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matLowest;
                    temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.2f, 1);
                }
            }
            else {
                Debug.Log("Key not found: " + p.Key);
            }

            // set Country Outline
            outlineKey = p.Key + "_outline";
            //Debug.Log(outlineKey);
            if (rootCountryOutlines.transform.Find(outlineKey).gameObject != null)
            {
                temp = rootCountryOutlines.transform.Find(outlineKey).gameObject;
                //temp = temp.transform.Find("default").gameObject;

                // set elevation
                if (p.Value > maxValue * 0.9f)
                {
                    temp.transform.localPosition = new Vector3(0, elevation * 0.5f * 0.005f, 0);
                }

                else if (p.Value > maxValue * 0.7f)
                {
                    temp.transform.localPosition = new Vector3(0, elevation * 0.4f * 0.005f, 0);
                }

                else if (p.Value > maxValue * 0.5f)
                {
                    temp.transform.localPosition = new Vector3(0, elevation * 0.3f * 0.005f, 0);
                }

                else if (p.Value > maxValue * 0.3f)
                {
                    temp.transform.localPosition = new Vector3(0, elevation * 0.2f * 0.005f, 0);
                }

                else if (p.Value > 0)
                {
                    temp.transform.localPosition = new Vector3(0, elevation * 0.1f * 0.005f, 0);
                }
            }
            else
            {
                Debug.Log("Key not found: " + outlineKey);
            }
        }

        // apply selection to items
        foreach (CollectionDataHandling.CollectionItem item in CollectionDataHandling.CollectionData.allItems)
        {
            temp = rootCollectionItems.transform.Find(item.objectRef).gameObject;
            temp.transform.localScale = new Vector3(scaleFactor, scaleFactor, collectionItemDepth);

            // get position
            tempXPosition = temp.transform.position.x;
            tempYPosition = temp.transform.position.y;
            tempZPosition = temp.transform.position.z;

            // set height
            countryValue = CollectionDataHandling.CollectionData.countryStats[item.country];

            if (countryValue > maxValue * 0.9f)
            {
                temp.transform.position = new Vector3(tempXPosition, tempYPosition + elevation * 0.5f, tempZPosition);
            }

            else if (countryValue > maxValue * 0.7f)
            {
                temp.transform.position = new Vector3(tempXPosition, tempYPosition + elevation * 0.4f, tempZPosition);
            }

            else if (countryValue > maxValue * 0.5f)
            {
                temp.transform.position = new Vector3(tempXPosition, tempYPosition + elevation * 0.3f, tempZPosition);
            }

            else if (countryValue > maxValue * 0.3f)
            {
                temp.transform.position = new Vector3(tempXPosition, tempYPosition + elevation * 0.2f, tempZPosition);
            }

            else if (countryValue > 0)
            {
                temp.transform.position = new Vector3(tempXPosition, tempYPosition + elevation * 0.1f, tempZPosition);
            }
        }
    }

    public void applySelection() {
        maxValue = CollectionDataHandling.CollectionData.getHighestSelectionDictionaryValue();

        // apply to country volumes
        foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStatsSelection)
        {
            temp = rootCountryVolumes.transform.Find(p.Key).gameObject;
            temp = temp.transform.Find("default").gameObject;

            // set color intervals & elevation
            if (p.Value > maxValue * 0.9f && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matHighest;
                temp.transform.localScale = new Vector3(1, defaultHeight + elevation, 1);
            }

            else if (p.Value > maxValue * 0.7f && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matHigh;
                temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.8f, 1);
            }

            else if (p.Value > maxValue * 0.5f && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matMedium;
                temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.6f, 1);
            }

            else if (p.Value > maxValue * 0.3f && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matLow;
                temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.4f, 1);
            }

            else if (p.Value > 0 && temp.GetComponent<MeshRenderer>() != null)
            {
                temp.GetComponent<MeshRenderer>().material = matLowest;
                temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.2f, 1);
            }
        }

        // apply selection to items
        foreach (CollectionDataHandling.CollectionItem item in CollectionDataHandling.CollectionData.selectedItems)
        {
            temp = rootCollectionItems.transform.Find(item.objectRef).gameObject;
            temp.transform.localScale = new Vector3(scaleFactor, scaleFactor, collectionItemDepth);

            // get position
            tempXPosition = temp.transform.position.x;
            tempYPosition = temp.transform.position.y;
            tempZPosition = temp.transform.position.z;

            // set height
            countryValue = CollectionDataHandling.CollectionData.countryStatsSelection[item.country];
            Debug.Log(countryValue);

            if (countryValue > maxValue * 0.9f)
            {
                temp.transform.position = new Vector3(tempXPosition, tempYPosition + elevation * 0.5f, tempZPosition);
            }

            else if (countryValue > maxValue * 0.7f)
            {
                temp.transform.position = new Vector3(tempXPosition, tempYPosition + elevation * 0.4f, tempZPosition);
            }

            else if (countryValue > maxValue * 0.5f)
            {
                temp.transform.position = new Vector3(tempXPosition, tempYPosition + elevation * 0.3f, tempZPosition);
            }

            else if (countryValue > maxValue * 0.3f)
            {
                temp.transform.position = new Vector3(tempXPosition, tempYPosition + elevation * 0.2f, tempZPosition);
            }

            else if (countryValue > 0)
            {
                temp.transform.position = new Vector3(tempXPosition, tempYPosition + elevation * 0.1f, tempZPosition);
            }
        }
    }
}
