using UnityEngine.UI;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class DisplayMetaInformation : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    public GameObject infopanel;
    private GameObject panel;
    private GameObject UIFront;
    private GameObject UIBack;
    private Text textFront;
    private Text textBack;
    private GameObject artistPhoto;

    private Material defaultMat;

    // set Tag for art objects
    private string artObject = "ArtObject";

    private bool panelActive = true;
    private bool touched = false;
    public static bool guideActive = true;


    public string ausstellungsText = "Willkommen im virtuellen Archiv der Ausstellung von [XXXX]. Die Ausstellung enthält Werke von [Name der Kunstschaffenden einfügen]. ";
    private string instruct = "<br><br>Zeigen Sie mit dem Pointer auf ein Kunstwerk für mehr Informationen zum Werk. ";
    private string welcome;
    private string currentObject = "Werk";
    private string currentArtist = "Artist";

    public GameObject artists;
    private int anzahlArtists;
    private GameObject[] allArtists;
    private GameObject artist;

    private Ray ray;
    private RaycastHit hit;
    private GameObject hitObject;
    public float rayDistance = 15.0f;

    private GameObject marker;
    private Color markerColor = new Color(0.97f, 0.97f, 0.97f, 0.5f);
    private Vector3 markerOffset;

    // x_move measures the movement on the trackpad
    // values range between -1 and 1
    private float x_move = 0.0f;
    private float x_current = 0.0f;
    private float x_movement;
    public float swipeSpeed = 150.0f;

    private float y_move = 0.0f;
    private float y_current = 0.0f;
    private float y_movement;
    public float scrollSpeed = 150.0f;


    void Start () {
        // set color of panel & assign it to panel
        defaultMat = (Material)Resources.Load("Timeline/Materials/DefaultMaterial", typeof(Material));
        defaultMat.color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        panel = infopanel.transform.Find("Panel").gameObject;
        panel.GetComponent<MeshRenderer>().material = defaultMat;
        artistPhoto = panel.transform.Find("ArtistPhoto").gameObject;
        artistPhoto.GetComponent<MeshRenderer>().material = defaultMat;

        // assign elements of panel to variable
        UIFront = panel.transform.Find("UIFront").gameObject;
        UIBack = panel.transform.Find("UIBack").gameObject;
        textFront = UIFront.GetComponentInChildren<Text>();
        textBack = UIBack.GetComponentInChildren<Text>();

        // create Marker that indicates when a Art Object is hit by ray
        marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        Collider c = marker.GetComponent<Collider>();
        c.enabled = false;
        MeshRenderer r = marker.GetComponent<MeshRenderer>();
        r.material.color = markerColor;
        r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        marker.SetActive(false);

        // count number of artists
        anzahlArtists = artists.transform.childCount;

        // create array for artists
        allArtists = new GameObject [anzahlArtists];

        // fill array with artists
        for (int i = 0; i < allArtists.Length; i++)
        {
            allArtists[i] = artists.transform.GetChild(i).gameObject;
        }

        // instantiante artist variable 
        artist = allArtists[0];

        // set text
        welcome = ausstellungsText + instruct;

        textFront.GetComponent<Text>().supportRichText = true;
        textFront.text = welcome.Replace("<br>", "\n");
        textBack.GetComponent<Text>().supportRichText = true;
        textBack.text = "  ";
    }


    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
 

        // shoot ray
        if (panelActive && device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            ray = new Ray();
            ray.origin = trackedObj.transform.position;
            ray.direction = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // retrieve metadata from hit object
                hitObject = hit.collider.gameObject;

                if (hitObject.tag == artObject)
                {
                    currentObject = hitObject.GetComponent<WerkInfo>().artist + "<br><b>" + hitObject.GetComponent<WerkInfo>().title + "</b><br>" + hitObject.GetComponent<WerkInfo>().year;

                    // check if there is an artist that matches with artist id
                    for (int i = 0; i < allArtists.Length; i++)
                    {
                        allArtists[i] = artists.transform.GetChild(i).gameObject;
                        if (hitObject.GetComponent<WerkInfo>().artist_id == allArtists[i].GetComponent<ArtistInfo>().artist_id) {
                            artist = allArtists[i];
                        }
                    }
                }

                // get metadata from the artist 
                /*
                // check if a picture material is set for artist
                if (artist.GetComponent<ArtistInfo>().picture == null)
                {
                    currentArtist = "<b>" + artist.GetComponent<ArtistInfo>().artistname + "</b><br>" + artist.GetComponent<ArtistInfo>().lebensdaten + "<br>" + artist.GetComponent<ArtistInfo>().bio;
                }
                else
                {
                    currentArtist = "<br><br><br><br><b>" + artist.GetComponent<ArtistInfo>().artistname + "</b><br>" + artist.GetComponent<ArtistInfo>().lebensdaten + "<br>" + artist.GetComponent<ArtistInfo>().bio;
                }
                */
                currentArtist = "<br><br><br><br><b>" + artist.GetComponent<ArtistInfo>().artistname + "</b><br>" + artist.GetComponent<ArtistInfo>().lebensdaten + "<br>" + artist.GetComponent<ArtistInfo>().bio;

                if (hit.collider.gameObject.tag == artObject)
                {
                    // show sphere at hit positon
                    /* TODO: maybe replace this with other additional pointer */
                    markerOffset = ray.direction;
                    markerOffset *= 0.04f;
                    marker.transform.position = (hit.point - markerOffset);
                    marker.SetActive(true);

                    // set text
                    textFront.text = currentObject.Replace("<br>", "\n");
                    textBack.text = currentArtist.Replace("<br>", "\n");

                    // set material for artist photo
                    /*
                    // check if a picture material is set for artist
                    if (artist.GetComponent<ArtistInfo>().picture != null)
                    {
                        artistPhoto.GetComponent<MeshRenderer>().material = artist.GetComponent<ArtistInfo>().picture;
                    }
                    else {
                        artistPhoto.GetComponent<MeshRenderer>().material = defaultMat;
                    }
                    */
                    artistPhoto.GetComponent<MeshRenderer>().material = artist.GetComponent<ArtistInfo>().picture;
                }
                
                // hide marker when ray hits object of different type
                else
                {
                    marker.SetActive(false);
                }
            }

            // hide marker when ray does not hit any object
            else
            {
                marker.SetActive(false);
            }
        }

        // hide marker 
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            marker.SetActive(false);
        }

        // allgemeine Ausstellungsinfos
        if (panelActive && device.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            textFront.text = welcome.Replace("<br>", "\n");
            textBack.text = "  ";
            artistPhoto.GetComponent<MeshRenderer>().material = defaultMat;
        }
    }


    void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        // show infopanel
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            // display panel
            if (!panelActive)
            {
                infopanel.SetActive(true);
                artistPhoto.SetActive(true);
                panelActive = true;
                guideActive = true;
            }
            // hide panel
            else
            {
                infopanel.SetActive(false);
                artistPhoto.SetActive(false);
                marker.SetActive(false);
                panelActive = false;
                guideActive = false;
            }
        }

        // Checks for Touchpad touch swipe & scroll
        if (panelActive && device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (!touched)
            {
                touched = true;
                x_move = device.GetAxis().x;
                y_move = device.GetAxis().y;
            }

            else
            {
                x_current = device.GetAxis().x;
                x_movement = x_move - x_current;
                y_current = device.GetAxis().y;
                y_movement = y_move - y_current;

                // detect if movement was swipe or scroll
                if (Mathf.Abs(x_movement)> Mathf.Abs(y_movement))
                {
                    // Set Rotation angle
                    Transform transform = infopanel.transform;
                    infopanel.transform.Rotate(0.0f, x_movement * swipeSpeed, 0.0f);
                    x_move = x_current;
                }
                else
                {
                    //TODO implement scroll movement for text
                    //y_movement contains scroll value
                }
            }
        }

        // reset x_move to zero when touch is released
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            x_move = 0.0f;
            y_move = 0.0f;
            touched = false;
        }
    }
}
