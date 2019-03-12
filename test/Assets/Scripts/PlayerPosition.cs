using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CollectionDataHandlingSpace;
using ScaleInteractionSpace;

public class PlayerPosition : MonoBehaviour {

    public GameObject player;
    public GameObject eyes;
    public GameObject countryVolumes;
    public GameObject controllcenterUI;
    private Text controllcenterText;
    private string currentText = "";

    private GameObject currentCountry;
    private string currentLocation;
    private float currentXCoordinate;
    private float currentYCoordinate;
    private string currentCountryName = "somewhere over the rainbow";

    private Vector3 currExactPos;
    private Vector3 currPos;
    private Vector3 prevPos;

    private Ray ray;
    private RaycastHit hit;
    private GameObject hitObject;
    public float rayDistance = 500.0f;

    // Use this for initialization
    void Start () {
        currPos = player.transform.position;

        // combine player & eyes position to get the real position of the user
        // because player position only changes with teleportation
        currExactPos = player.transform.position + eyes.transform.position;

        // set controllcenter text
        controllcenterText = controllcenterUI.GetComponentInChildren<Text>();
        controllcenterText.GetComponent<Text>().supportRichText = true;
        currentText = "position: " + currentText;
        controllcenterText.text = currentText.Replace("<br>", "\n");
    }


    // Update is called once per frame
    void Update()
    {
        prevPos = currPos;
        currPos = player.transform.position;
        currExactPos = player.transform.position + eyes.transform.position;

        // update controllcenter content
        currentText = "position: " + convertPositionToGeoCoordinate(currExactPos) + "<br>" + 
            "altitude: " + ScaleInteract.currentStaticScaleFactor + " above " + currentCountryName + "<br>" + 
            "selected items: " + CollectionDataHandling.CollectionData.countryStats.Count + " / " + CollectionDataHandling.CollectionData.countryStatsSelection.Count;

        // set new text
        controllcenterText.text = currentText.Replace("<br>", "\n");

        if (currPos != prevPos)
        {
            //Debug.Log("Location changed");
            // TODO: check for country volume below


            RaycastHit hit;

            Debug.Log(player.transform.position + eyes.transform.position + transform.TransformDirection(Vector3.up));
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(player.transform.position + eyes.transform.position + transform.TransformDirection(Vector3.up), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(player.transform.position + eyes.transform.position + transform.TransformDirection(Vector3.up), transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                Debug.Log("Did Hit " + hit.collider.gameObject);
                //currentCountryName = hit.collider;
                // TODO check what was hit !!!!!!
            }
            else
            {
                Debug.Log("Did not Hit");
            }

            /*

            ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                Debug.Log("hit object: " + hit.collider);
                

            }*/

            /*
            //Debug.DrawRay(position, direction, Color.green);
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // hit object
                hitObject = hit.collider.gameObject;
                Debug.Log(hitObject);

                // TODO check if UI is already visible, only call the following code once

                if (hitObject.tag == "Volume")
                {
                    currentCountry = hitObject;
                    Debug.Log(hitObject);
                }
            }
            */


            /*
            if (Physics.Raycast(player.transform.position, Vector3.down, 500)) {
                print("There is something in front of the object!");
            }
            */

            /*
            ray = new Ray();
            ray.origin = player.transform.position;
            ray.direction = transform.TransformDirection(Vector3.down);

            if (Physics.Raycast(ray, out hit, 500))
            {
                // retrieve hit object
                hitObject = hit.collider.gameObject;
                Debug.Log(hitObject.name);

                if (hitObject.tag == Volume)
                {
                    currentCountry = hitObject;
                    currentLocation = currentCountry.name;
                    //Debug.Log(currentLocation);
                }

            }
            */
        }
    }

    // convert position to Geocoordinate
    public string convertPositionToGeoCoordinate(Vector3 pos)
    {
        // x coordinates go from -512 to +512 and have to be mapped to -180 to +180
        float xCoord = pos.x * 180 / 512;
        float xDegree = Mathf.Floor(Mathf.Abs(xCoord));
        float diff = Mathf.Abs(xCoord) - xDegree;
        float xMin = Mathf.Floor(diff * 90);
        float xSec = Mathf.Floor((diff * 90 - xMin) * 90);

        // output as string
        string xString;
        if (xCoord > 0)
        {
            xString = xDegree + "° " + xMin + "' " + xSec + "'' E";
        }
        else {
            xString = xDegree + "° " + xMin + "' " + xSec + "'' W";
        }

        // x coordinates go from approx -320 to +320 and have to be mapped to -90 to +90
        // 90 --> approx 30deg
        // 140 --> approx 45deg
        // 195 --> approx 60deg
        // 275 --> approx 75deg

        // TODO better conversion
        float yCoord = pos.z / 3.2f;

        if (yCoord > 90) {
            yCoord = 90;
        }

        if (yCoord < -90)
        {
            yCoord = -90;
        }

        float yDegree = Mathf.Floor(Mathf.Abs(yCoord));
        diff = Mathf.Abs(yCoord) - yDegree;
        float yMin = Mathf.Floor(diff * 90);
        float ySec = Mathf.Floor((diff * 90 - yMin) * 90);

        // output as string
        string yString;
        if (yCoord > 0)
        {
            yString = yDegree + "° " + yMin + "' " + ySec + "'' N";
        }
        else
        {
            yString = yDegree + "° " + yMin + "' " + ySec + "'' S";
        }

        // output should have the format XX° XX' XX'' NS / XX° XX' XX'' EW
        return yString + " / " + xString;
    }
}
