﻿using System.Collections;
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
            "altitude: " + ScaleInteract.currentZoomLevel + " / " + ScaleInteract.currentStaticScaleFactor + "<br>" + 
            "selected items: " + CollectionDataHandling.CollectionData.countryStats.Count + " / " + CollectionDataHandling.CollectionData.countryStatsSelection.Count;

        // set new text
        controllcenterText.text = currentText.Replace("<br>", "\n");

        if (currPos != prevPos)
        {
            //Debug.Log("Location changed");
            // TODO: check for country volume below

            //Vector3 fwd = transform.TransformDirection(Vector3.forward);

            Vector3 down = transform.TransformDirection(Vector3.forward) * 300;
            Debug.DrawRay(transform.position, down, Color.green);
            Debug.DrawRay(player.transform.position, down, Color.red);

            RaycastHit hit;

            Debug.Log(player.transform.position);
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(player.transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(player.transform.position, transform.TransformDirection(Vector3.up) * 1000, Color.white);
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

        float y = pos.y;

        // TODO transform position into proper GeoCoordinate
        // output should have the format XX° XX' XX'' NS / XX° XX' XX'' EW
        // x --> EW?!?
        // y --> NS?!?
        
        // multiply by given factor to get correct value (max x should be +- 180, max y +- 90

        // x coordinate: -512 to +512
        // format it
        // distinguish positive/negative
        // set direction and add output as abs

        return pos.x + " / " + pos.z;
    }
}