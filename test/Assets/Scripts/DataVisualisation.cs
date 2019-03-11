using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CollectionDataHandlingSpace;
using ScaleInteractionSpace;

/* *
 * 
 * this script needs to be attached to the controller. 
 * for some unexplainable reason it doesn't work if it is attached to an empty child object.
 * 
 * */

namespace DataVisualisationSpace { 

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

        private float collectionItemHeight = 0.8f; // 1.1f;
        private float countryHeight;
        private float outlineHeight = 0.05f;

        public GameObject rootCountryVolumes;
        public GameObject rootCountryOutlines;
        public GameObject rootCountryLabels;
        public GameObject rootCollectionItems;
        private GameObject temp;

        private float tempXPosition;
        private float tempYPosition;
        private float tempZPosition;

        private int maxValue;
        private int countryValue;

        private string outlineKey;

        private int currZoomLevel;
        private int prevZoomLevel;

        public float zoomFactorItemsLevel1 = 3.0f;
        public float zoomFactorItemsLevel2 = 2.0f;
        public float zoomFactorItemsLevel3 = 1.0f;

        private float zoomFactorItems;


        // Use this for initialization
        void Start()
        {
            //Debug.Log( "Data Visualization called for the first time");

            // set materials
            /*
            matHighest = (Material)Resources.Load("Colours/HighestNoReflection", typeof(Material));
            matHigh = (Material)Resources.Load("Colours/HighNoReflection", typeof(Material));
            matMedium = (Material)Resources.Load("Colours/MediumNoReflection", typeof(Material));
            matLow = (Material)Resources.Load("Colours/LowNoReflections", typeof(Material));
            matLowest = (Material)Resources.Load("Colours/LowestNoReflection", typeof(Material));
            */

            // set zoom level/factor
            currZoomLevel = ScaleInteract.currentZoomLevel;
            //UpdateZoomLevel(currZoomLevel);

            // set to zoom level 1 to start with by default
            zoomFactorItems = zoomFactorItemsLevel1;

            // apply data visualisation to the dataset as a whole 
            // (no particular selection)
            // applyDataVisualisation();

            // all items selected at start
            // consequently all subregions in FilterInteract.cs need to be set to true 
            // and materials set to selectedMaterial
            CollectionDataHandling.CollectionData.selectItemsByRegion(true, true, true, true, true);
            applySelection();
        }

        // Update is called once per frame
        void Update()
        {
            prevZoomLevel = currZoomLevel;
            currZoomLevel = ScaleInteract.currentZoomLevel;

            // check if zoom level has changed
            if (prevZoomLevel != currZoomLevel) {
                updateZoomLevel(currZoomLevel);
            }
        }

        // put all country volumes & outlines & collection items back to default position
        public void resetDataVisualisation() {
            //Debug.Log("Reset Data Visualisation");

            // reset country volumes to default
            foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStats)
            {
                // reset Country Volume
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
                    Debug.Log("Country Volume Key not found: " + p.Key);
                }

                // hide Country Labels
                if (rootCountryLabels.transform.Find(p.Key).gameObject != null)
                {
                    temp = rootCountryLabels.transform.Find(p.Key).gameObject;
                    temp.SetActive(false);
                }
                else
                {
                    Debug.Log("Country Label Key not found: " + p.Key);
                }
                
                // reset Country Outline
                outlineKey = p.Key + "_outline";
                if (rootCountryOutlines.transform.Find(outlineKey).gameObject != null)
                {
                    temp = rootCountryOutlines.transform.Find(outlineKey).gameObject;

                    // reset position
                    temp.transform.localPosition = new Vector3(0, 0, 0);
                }
                else
                {
                    Debug.Log("Country Outline Key not found: " + outlineKey);
                }
            }

            // reset collection items to default, taking zoom level into acount
            resetDataVisualisationCollectionItems();
        }


        public void resetDataVisualisationCollectionItems() {
            //Debug.Log("Reset Data Visualisation of Collection Items");

            foreach (CollectionDataHandling.CollectionItem item in CollectionDataHandling.CollectionData.allItems)
            {
                temp = rootCollectionItems.transform.Find(item.objectRef).gameObject;
                // reset size
                temp.transform.localScale = new Vector3(getZoomFactorItems(), getZoomFactorItems(), collectionItemDepth);

                // resize height
                tempXPosition = temp.transform.position.x;
                tempZPosition = temp.transform.position.z;
                temp.transform.position = new Vector3(tempXPosition, collectionItemHeight, tempZPosition);
            }
        }


        // apply data visualisation to all items, no selection
        public void applyDataVisualisation() {
            //Debug.Log("Apply Data Visualisation to all items");
            maxValue = CollectionDataHandling.CollectionData.getHighestDictionaryValue();

            // apply to country volumes
            foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStats)
            {
                // set country volume height
                setCountryVolumeHeight(p.Key, p.Value, maxValue);

                // show label
                if (rootCountryLabels.transform.Find(p.Key).gameObject != null)
                {
                    temp = rootCountryLabels.transform.Find(p.Key).gameObject;
                    temp.SetActive(true);
                }
                else
                {
                    Debug.Log("Country Label Key not found: " + p.Key);
                }

                // set Country Outline
                outlineKey = p.Key + "_outline";
                setCountryOutlineHeight(outlineKey, p.Value, maxValue);
            }

            // apply selection to items
            applyDataVisualizationCollectionItems();
        }


        // apply data visualisation to selected items
        public void applySelection() {
            //Debug.Log("Apply Data Visualisation to selected items");
            maxValue = CollectionDataHandling.CollectionData.getHighestSelectionDictionaryValue();

            // apply to country volumes
            foreach (KeyValuePair<string, int> p in CollectionDataHandling.CollectionData.countryStatsSelection)
            {
                //set country volume height
                setCountryVolumeHeight(p.Key, p.Value, maxValue);

                // show label
                if (rootCountryLabels.transform.Find(p.Key).gameObject != null)
                {
                    Debug.Log("activate Label " + p.Key);
                    temp = rootCountryLabels.transform.Find(p.Key).gameObject;
                    temp.SetActive(true);
                }
                else
                {
                    Debug.Log("Country Label Key not found: " + p.Key);
                }

                // set Country Outline
                outlineKey = p.Key + "_outline";
                setCountryOutlineHeight(outlineKey, p.Value, maxValue);
            }

            // apply selection to items
            applySelectionCollectionItems();
        }


        public void applyDataVisualizationCollectionItems() {
            //Debug.Log("Apply Data Visualisation to Collection Items");

            maxValue = CollectionDataHandling.CollectionData.getHighestDictionaryValue();

            foreach (CollectionDataHandling.CollectionItem item in CollectionDataHandling.CollectionData.allItems)
            {
                // apply to collection items
                setCollectionItemProperties(item, maxValue, 1.0f, false);
            }
        }


        public void applySelectionCollectionItems() {
            //Debug.Log("Apply Data Visualisation to selected Collection Items");

            maxValue = CollectionDataHandling.CollectionData.getHighestSelectionDictionaryValue();

            foreach (CollectionDataHandling.CollectionItem item in CollectionDataHandling.CollectionData.selectedItems)
            {
                // apply to collection items
                setCollectionItemProperties(item, maxValue, scaleFactor, true);
            }
        }


        public void setCountryVolumeHeight(string key, int value, int max) {
            //set country volume height
            if (rootCountryVolumes.transform.Find(key).gameObject != null)
            {
                temp = rootCountryVolumes.transform.Find(key).gameObject;
                temp = temp.transform.Find("default").gameObject;

                // set color intervals & elevation
                if (value > max * 0.9f && temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matHighest;
                    temp.transform.localScale = new Vector3(1, defaultHeight + elevation, 1);
                }

                else if (value > max * 0.7f && temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matHigh;
                    temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.8f, 1);
                }

                else if (value > max * 0.5f && temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matMedium;
                    temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.6f, 1);
                }

                else if (value > max * 0.3f && temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matLow;
                    temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.4f, 1);
                }

                else if (value > 0 && temp.GetComponent<MeshRenderer>() != null)
                {
                    temp.GetComponent<MeshRenderer>().material = matLowest;
                    temp.transform.localScale = new Vector3(1, defaultHeight + elevation * 0.2f, 1);
                }
            }
            else
            {
                Debug.Log("Country Volume Key not found: " + key);
            }
        }


        public void setCountryOutlineHeight(string key, int value, int max)
        {
            // set Country Outline
            if (rootCountryOutlines.transform.Find(key).gameObject != null)
            {
                temp = rootCountryOutlines.transform.Find(key).gameObject;

                // set elevation
                if (value > max * 0.9f)
                {
                    temp.transform.localPosition = new Vector3(0, elevation * 0.5f * 0.005f, 0);
                }

                else if (value > max * 0.7f)
                {
                    temp.transform.localPosition = new Vector3(0, elevation * 0.4f * 0.005f, 0);
                }

                else if (value > max * 0.5f)
                {
                    temp.transform.localPosition = new Vector3(0, elevation * 0.3f * 0.005f, 0);
                }

                else if (value > max * 0.3f)
                {
                    temp.transform.localPosition = new Vector3(0, elevation * 0.2f * 0.005f, 0);
                }

                else if (value > 0)
                {
                    temp.transform.localPosition = new Vector3(0, elevation * 0.1f * 0.005f, 0);
                }
            }
            else
            {
                Debug.Log("Country Outline Key not found: " + outlineKey);
            }
        }


        public void setCollectionItemProperties(CollectionDataHandling.CollectionItem item, int max, float scale, bool selection)
        {
            temp = rootCollectionItems.transform.Find(item.objectRef).gameObject;
            temp.transform.localScale = new Vector3(scale * getZoomFactorItems(), scale * getZoomFactorItems(), collectionItemDepth);

            // get position
            tempXPosition = temp.transform.position.x;
            tempZPosition = temp.transform.position.z;

            // set country value depending if general visualization or selected objects only
            if (selection) {
                countryValue = CollectionDataHandling.CollectionData.countryStatsSelection[item.country];
            }
            else {
                countryValue = CollectionDataHandling.CollectionData.countryStats[item.country];
            }

            // set height
            if (countryValue > max * 0.9f)
            {
                temp.transform.position = new Vector3(tempXPosition, (float)(collectionItemHeight + elevation * 0.5f + scale * getZoomFactorItems() * 0.5f), tempZPosition);
            }

            else if (countryValue > max * 0.7f)
            {
                temp.transform.position = new Vector3(tempXPosition, (float)(collectionItemHeight + elevation * 0.4f + scale * getZoomFactorItems() * 0.5f), tempZPosition);
            }

            else if (countryValue > max * 0.5f)
            {
                temp.transform.position = new Vector3(tempXPosition, (float)(collectionItemHeight + elevation * 0.3f + scale * getZoomFactorItems() * 0.5f), tempZPosition);
            }

            else if (countryValue > max * 0.3f)
            {
                temp.transform.position = new Vector3(tempXPosition, (float)(collectionItemHeight + elevation * 0.2f + scale * getZoomFactorItems() * 0.5f), tempZPosition);
            }

            else if (countryValue > 0)
            {
                temp.transform.position = new Vector3(tempXPosition, (float)(collectionItemHeight + elevation * 0.1f + scale * getZoomFactorItems() * 0.5f), tempZPosition);
            }
        }


        public void updateZoomLevel(int z) {
            //Debug.Log("Update ZoomLevel");

            if (z == 1) {
                zoomFactorItems = zoomFactorItemsLevel1;
            }

            if (z == 2)
            {
                zoomFactorItems = zoomFactorItemsLevel2;
            }

            if (z == 3)
            {
                zoomFactorItems = zoomFactorItemsLevel3;
            }

            else {
                Debug.Log("Invalid ZoomLevel: " + z);
            }

            // apply data visualisation to all collection items, however not to country volumes & outlines
            applyDataVisualizationCollectionItems();

            //  if there are countries selected, apply data visualization to selected items
            if (CollectionDataHandlingSpace.CollectionDataHandling.CollectionData.selectedItems.Count > 0)
            {
                applySelectionCollectionItems();
            }
        }


        public float getZoomFactorItems() {
            return zoomFactorItems;
        }

    }
}