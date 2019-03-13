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

        // get current position based on body & head movement
        // doesn't work properly with teleportation...
        /*
        ray = new Ray();
        ray.origin = player.transform.position + eyes.transform.position;
        ray.direction = transform.TransformDirection(Vector3.down);

        hits = Physics.RaycastAll(player.transform.position + eyes.transform.position, Vector3.down, Mathf.Infinity);
        currentCountryName = "the world";

        foreach (RaycastHit rch in hits)
        {
            if (rch.collider.gameObject.tag == "Volume")
            {
                currentCountry = rch.collider.gameObject;
                currentCountryName = currentCountry.transform.parent.name;
            }
        }
        */

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
            currentCountryName = "the world";

            foreach (RaycastHit rch in hits) {
                if (rch.collider.gameObject.tag == "Volume") {
                    currentCountry = rch.collider.gameObject;
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
        countryCodes.Add("CD", "Democratic Republic of Congo");
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
        countryCodes.Add("FJ", "Fiji");
        countryCodes.Add("FK", "Falkland Islands ");
        countryCodes.Add("FR", "France");
        countryCodes.Add("GA", "Gabon");
        countryCodes.Add("GB", "United Kingdom");
        countryCodes.Add("GE", "Georgia");
        countryCodes.Add("GF", "French Guiana");
        countryCodes.Add("GH", "Ghana");
        countryCodes.Add("GL", "Greenland");
        countryCodes.Add("GM", "Gambia");
        countryCodes.Add("GN", "Guinea");
        countryCodes.Add("GQ", "Equatorial Guinea");
        countryCodes.Add("GR", "Greece");
        countryCodes.Add("GS", "South Georgia and the South Sandwich Islands");
        countryCodes.Add("GT", "Guatemala");
        countryCodes.Add("GW", "Guinea-Bissau");
        countryCodes.Add("GY", "Guyana");
        countryCodes.Add("HN", "Honduras");
        countryCodes.Add("HR", "Croatia");
        countryCodes.Add("HT", "Haiti");
        countryCodes.Add("HU", "Hungary");
        countryCodes.Add("ID", "Indonesia");
        countryCodes.Add("IE", "Ireland");
        countryCodes.Add("IL", "Israel");
        countryCodes.Add("IN", "India");
        countryCodes.Add("IQ", "Iraq");
        countryCodes.Add("IR", "Iran");
        countryCodes.Add("IS", "Iceland");
        countryCodes.Add("IT", "Italy");
        countryCodes.Add("JM", "Jemen");
        countryCodes.Add("JO", "Jordan");
        countryCodes.Add("JP", "Japan");
        countryCodes.Add("KE", "Kenya");
        countryCodes.Add("KG", "Kyrgyzstan");
        countryCodes.Add("KH", "Cambodia");
        countryCodes.Add("KP", "North Korea");
        countryCodes.Add("KR", "South Korea");
        countryCodes.Add("KW", "Kuwait");
        countryCodes.Add("KZ", "Kazakhstan");
        countryCodes.Add("LA", "Laos");
        countryCodes.Add("LB", "Lebanon");
        countryCodes.Add("LK", "Sri Lanka");
        countryCodes.Add("LR", "Liberia");
        countryCodes.Add("LS", "Lesotho");
        countryCodes.Add("LT", "Lithuania");
        countryCodes.Add("LU", "Luxembourg");
        countryCodes.Add("LV", "Latvia");
        countryCodes.Add("LY", "Libya");
        countryCodes.Add("MD", "Moldova");
        countryCodes.Add("ME", "Montenegro");
        countryCodes.Add("MG", "Madagascar");
        countryCodes.Add("MK", "Macedonia");
        countryCodes.Add("ML", "Mali");
        countryCodes.Add("MM", "Myanmar");
        countryCodes.Add("MN", "Mongolia");
        countryCodes.Add("MO", "Morocco"); // sometimes the abbreviation MA is used for Morocco and MO can also apply to Macao
        countryCodes.Add("MR", "Mauritania");
        countryCodes.Add("MW", "Malawi");
        countryCodes.Add("MX", "Mexico");
        countryCodes.Add("MY", "Malaysia");
        countryCodes.Add("MZ", "Mozambique");
        countryCodes.Add("NA", "Namibia");
        countryCodes.Add("NC", "New Caledonia");
        countryCodes.Add("NE", "Niger");
        countryCodes.Add("NG", "Nigeria");
        countryCodes.Add("NI", "Nicaragua");
        countryCodes.Add("NL", "Netherlands");
        countryCodes.Add("NO", "Norway");
        countryCodes.Add("NP", "Nepal");
        countryCodes.Add("NZ", "New Zealand");
        countryCodes.Add("OM", "Oman");
        countryCodes.Add("PA", "Panama");
        countryCodes.Add("PE", "Peru");
        countryCodes.Add("PG", "Papua New Guinea");
        countryCodes.Add("PH", "Philippines");
        countryCodes.Add("PK", "Pakistan");
        countryCodes.Add("PL", "Poland");
        countryCodes.Add("PR", "Puerto Rico");
        countryCodes.Add("PS", "Palestine");
        countryCodes.Add("PT", "Portugal");
        countryCodes.Add("PY", "Paraguay");
        countryCodes.Add("QA", "Qatar");
        countryCodes.Add("RO", "Romania");
        countryCodes.Add("RS", "Serbia");
        countryCodes.Add("RU", "Russia");
        countryCodes.Add("RW", "Rwanda");
        countryCodes.Add("SA", "Saudi Arabia");
        countryCodes.Add("SB", "Solomon Islands");
        countryCodes.Add("SD", "Sudan");
        countryCodes.Add("SE", "Sweden");
        countryCodes.Add("SI", "Slovenia");
        countryCodes.Add("SK", "Slovakia");
        countryCodes.Add("SL", "Sierra Leone");
        countryCodes.Add("SN", "Senegal");
        countryCodes.Add("SO", "Somalia");
        countryCodes.Add("SR", "Suriname");
        countryCodes.Add("SV", "El Salvador");
        countryCodes.Add("SY", "Syria");
        countryCodes.Add("SZ", "Swaziland");
        countryCodes.Add("TD", "Chad");
        countryCodes.Add("TF", "French Southern Territories");
        countryCodes.Add("TG", "Togo");
        countryCodes.Add("TH", "Thailand");
        countryCodes.Add("TJ", "Tajikistan");
        countryCodes.Add("TL", "Timor-Leste");
        countryCodes.Add("TM", "Turkmenistan");
        countryCodes.Add("TN", "Tunisia");
        countryCodes.Add("TR", "Turkey");
        countryCodes.Add("TT", "Trinidad and Tobago");
        countryCodes.Add("TZ", "Tanzania");
        countryCodes.Add("UA", "Ukraine");
        countryCodes.Add("UG", "Uganda");
        countryCodes.Add("US", "United States of America");
        countryCodes.Add("UY", "Uruguay");
        countryCodes.Add("UZ", "Uzbekistan");
        countryCodes.Add("VE", "Venezuela");
        countryCodes.Add("VN", "Vietnam");
        countryCodes.Add("YE", "Yemen");
        countryCodes.Add("ZA", "South Africa");
        countryCodes.Add("ZM", "Zambia");
        countryCodes.Add("ZW", "Zimbabwe");

        Debug.Log("Country Codes Dictionary created with " + countryCodes.Count + " items");
    }
}
