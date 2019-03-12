using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CollectionDataHandlingSpace;
using ScaleInteractionSpace;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PlayerPosition : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

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
    private string currentCountryName = "the world";

    private Vector3 currExactPos;
    private Vector3 currPos;
    private Vector3 prevPos;

    private Ray ray;
    private RaycastHit hit;
    private RaycastHit[] hits;
    private GameObject hitObject;
    public float rayDistance = 500.0f;

    public GameObject cube;


    private Dictionary<string, string> countryCodes;


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

        createCountrycodesDictionary();
    }


    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    // Update is called once per frame
    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        // get current position
        ray = new Ray();
        ray.origin = player.transform.position + eyes.transform.position;
        ray.direction = transform.TransformDirection(Vector3.down);

        hits = Physics.RaycastAll(player.transform.position + eyes.transform.position, Vector3.down, Mathf.Infinity);

        foreach (RaycastHit rch in hits)
        {
            if (rch.collider.gameObject.tag == "Volume")
            {
                currentCountry = rch.collider.gameObject;
                currentCountryName = currentCountry.transform.parent.name;
            }
        }

        /*
        if (currPos != prevPos) {
            Debug.Log("Location changed");
        }
        */

        prevPos = currPos;
        currPos = player.transform.position;
        currExactPos = player.transform.position + eyes.transform.position;

        // update controllcenter content
        currentText = "position: " + convertPositionToGeoCoordinate(currExactPos) + "<br>" + 
            "altitude: " + ScaleInteract.currentStaticScaleFactor + " x above " + currentCountryName + " <br>" + 
            "items in current selection: " + CollectionDataHandling.CollectionData.countryStatsSelection.Count;

        // set new text
        controllcenterText.text = currentText.Replace("<br>", "\n");

        //cube.transform.localPosition = new Vector3(currExactPos.x, 2.0f, currExactPos.z);
        

        // get postion when releasing Trigger
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {            
            // shoot ray
            ray = new Ray();
            ray.origin = trackedObj.transform.position;
            ray.direction = transform.TransformDirection(Vector3.forward);

            hits = Physics.RaycastAll(ray, rayDistance);

            foreach (RaycastHit rch in hits) {
                if (rch.collider.gameObject.tag == "Volume") {
                    currentCountry = rch.collider.gameObject;
                    //currentCountryName = currentCountry.transform.parent.name;
                    currentCountryName = getCountryName(currentCountry.transform.parent.name);
                }
            }
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


    public string getCountryName(string countrycode)
    {
        if (countryCodes.ContainsKey(countrycode)) {
            return countryCodes[countrycode];
        }
        return "the world";
    }


    public void createCountrycodesDictionary() {
        countryCodes = new Dictionary<string, string>();

        // https://www.nationsonline.org/oneworld/country_code_list.htm
        countryCodes.Add("AE", "United Arab Emirates");
        countryCodes.Add("AF", "Afghanistan");
        countryCodes.Add("AL", "Albania");
        countryCodes.Add("AM", "Armenia");
        countryCodes.Add("AO", "Angola");
        countryCodes.Add("AR", "Argentina");
        countryCodes.Add("AT", "Austria");
        countryCodes.Add("AU", "Australia");
        countryCodes.Add("AZ", "Azerbaijan");
        countryCodes.Add("BA", "Bosnia and Herzegovina");
        countryCodes.Add("BD", "Bangladesh");
        countryCodes.Add("BE", "Belgium");
        countryCodes.Add("BF", "Burkina Faso");
        countryCodes.Add("BG", "Bulgaria");
        countryCodes.Add("BI", "Burundi");
        countryCodes.Add("BJ", "Benin");
        countryCodes.Add("BN", "Brunei Darussalam");
        countryCodes.Add("BO", "Bolivia");
        countryCodes.Add("BR", "Brazil");
        countryCodes.Add("BS", "Bahamas");
        countryCodes.Add("BT", "Bhutan");
        countryCodes.Add("BW", "Botswana");
        countryCodes.Add("BY", "Belarus");
        countryCodes.Add("BZ", "Belize");
        countryCodes.Add("CA", "Canada");
        countryCodes.Add("CD", "Democratic Republic of the Congo");
        countryCodes.Add("CF", "Central African Republic");
        countryCodes.Add("CG", "Congo");
        countryCodes.Add("CH", "Switzerland");
        countryCodes.Add("CI", "Côte d'Ivoire");
        countryCodes.Add("CL", "Chile");
        countryCodes.Add("CM", "Cameroon");
        countryCodes.Add("CN", "China");
        countryCodes.Add("CO", "Colombia");
        countryCodes.Add("CR", "Costa Rica");
        countryCodes.Add("CU", "Cuba");
        countryCodes.Add("CY", "Cyprus");
        countryCodes.Add("CZ", "Czech Republic");
        countryCodes.Add("DE", "Germany");
        countryCodes.Add("DJ", "Djibouti");
        countryCodes.Add("DK", "Denmark");
        countryCodes.Add("DO", "Dominican Republic");
        countryCodes.Add("DZ", "Algeria");
        countryCodes.Add("EC", "Ecuador");
        countryCodes.Add("EE", "Estonia");
        countryCodes.Add("EG", "Egypt");
        countryCodes.Add("EH", "Western Sahara");
        countryCodes.Add("ER", "Eritrea");
        countryCodes.Add("ES", "Spain");
        countryCodes.Add("ET", "Ethiopia");
        countryCodes.Add("FI", "Finland");
        countryCodes.Add("FJ", " ");
        countryCodes.Add("FK", " ");
        countryCodes.Add("FR", "France");
        countryCodes.Add("GA", " ");
        countryCodes.Add("GB", " ");
        countryCodes.Add("GE", " ");
        countryCodes.Add("GF", " ");
        countryCodes.Add("GH", " ");
        countryCodes.Add("GL", " ");
        countryCodes.Add("GM", " ");
        countryCodes.Add("GN", " ");
        countryCodes.Add("GQ", " ");
        countryCodes.Add("GR", " ");
        countryCodes.Add("GS", " ");
        countryCodes.Add("GT", " ");
        countryCodes.Add("GW", " ");
        countryCodes.Add("GY", " ");
        countryCodes.Add("HN", " ");
        countryCodes.Add("HR", " ");
        countryCodes.Add("HT", " ");
        countryCodes.Add("HU", " ");
        countryCodes.Add("ID", " ");
        countryCodes.Add("IE", " ");
        countryCodes.Add("IL", " ");
        countryCodes.Add("IN", "India");
        countryCodes.Add("IQ", " ");
        countryCodes.Add("IR", "Iran");
        countryCodes.Add("IS", " ");
        countryCodes.Add("IT", "Italy");
        countryCodes.Add("JM", "Jemen");
        countryCodes.Add("JO", "Jordan");
        countryCodes.Add("JP", "Japan");
        countryCodes.Add("KE", " ");
        countryCodes.Add("KG", " ");
        countryCodes.Add("KH", "Cambodia");
        countryCodes.Add("KP", " ");
        countryCodes.Add("KR", " ");
        countryCodes.Add("KW", " ");
        countryCodes.Add("KZ", " ");
        countryCodes.Add("LA", " ");
        countryCodes.Add("LB", " ");
        countryCodes.Add("LK", " ");
        countryCodes.Add("LR", " ");
        countryCodes.Add("LS", " ");
        countryCodes.Add("LT", " ");
        countryCodes.Add("LU", " ");
        countryCodes.Add("LV", " ");
        countryCodes.Add("LY", " ");
        countryCodes.Add("MD", " ");
        countryCodes.Add("ME", " ");
        countryCodes.Add("MG", " ");
        countryCodes.Add("MK", " ");
        countryCodes.Add("ML", "Mali");
        countryCodes.Add("MM", " ");
        countryCodes.Add("MN", " ");
        countryCodes.Add("MO", " ");
        countryCodes.Add("MR", " ");
        countryCodes.Add("MW", " ");
        countryCodes.Add("MX", " ");
        countryCodes.Add("MY", " ");
        countryCodes.Add("MZ", " ");
        countryCodes.Add("NA", " ");
        countryCodes.Add("NC", " ");
        countryCodes.Add("NE", " ");
        countryCodes.Add("NG", " ");
        countryCodes.Add("NI", " ");
        countryCodes.Add("NL", " ");
        countryCodes.Add("NO", " ");
        countryCodes.Add("NP", " ");
        countryCodes.Add("NZ", " ");
        countryCodes.Add("OM", " ");
        countryCodes.Add("PA", " ");
        countryCodes.Add("PE", "Peru");
        countryCodes.Add("PG", " ");
        countryCodes.Add("PH", " ");
        countryCodes.Add("PK", "Pakistan");
        countryCodes.Add("PL", " ");
        countryCodes.Add("PR", " ");
        countryCodes.Add("PS", " ");
        countryCodes.Add("PT", " ");
        countryCodes.Add("PY", " ");
        countryCodes.Add("QA", " ");
        countryCodes.Add("RO", " ");
        countryCodes.Add("RS", " ");
        countryCodes.Add("RU", " ");
        countryCodes.Add("RW", " ");
        countryCodes.Add("SA", " ");
        countryCodes.Add("SB", " ");
        countryCodes.Add("SD", " ");
        countryCodes.Add("SE", " ");
        countryCodes.Add("SI", " ");
        countryCodes.Add("SK", " ");
        countryCodes.Add("SL", " ");
        countryCodes.Add("SN", " ");
        countryCodes.Add("SO", " ");
        countryCodes.Add("SR", " ");
        countryCodes.Add("SV", "El Salvador");
        countryCodes.Add("SY", " ");
        countryCodes.Add("SZ", " ");
        countryCodes.Add("TD", " ");
        countryCodes.Add("TF", " ");
        countryCodes.Add("TG", " ");
        countryCodes.Add("TH", "Thailand");
        countryCodes.Add("TJ", " ");
        countryCodes.Add("TL", " ");
        countryCodes.Add("TM", " ");
        countryCodes.Add("TN", " ");
        countryCodes.Add("TR", " ");
        countryCodes.Add("TT", " ");
        countryCodes.Add("TZ", " ");
        countryCodes.Add("UA", " ");
        countryCodes.Add("UG", " ");
        countryCodes.Add("US", "United States");
        countryCodes.Add("UY", " ");
        countryCodes.Add("UZ", " ");
        countryCodes.Add("VE", " ");
        countryCodes.Add("VN", "Vietnam");
        countryCodes.Add("YE", " ");
        countryCodes.Add("ZA", " ");
        countryCodes.Add("ZM", " ");
        countryCodes.Add("ZW", " ");

        Debug.Log("Country Codes Dictionary created with " + countryCodes.Count + " items");
    }
}
