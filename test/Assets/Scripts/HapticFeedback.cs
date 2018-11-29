// this script should get attached to the camera eyes
// objects that call for haptic feedback need the tag "Wall"


using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class HapticFeedback : MonoBehaviour {

    SteamVR_TrackedObject trackedObjLeft;
    SteamVR_TrackedObject trackedObjRight;
    SteamVR_Controller.Device deviceLeft;
    SteamVR_Controller.Device deviceRight;

    public GameObject controllerLeft;
    public GameObject controllerRight;
  
    private bool proviceFeedback = false;

    public float fadeSpeed = 1.0f; 
    private bool fade = true;


    // fade in & out Script von http://wiki.unity3d.com/index.php/FadeInOut

    // Style for background tiling
    private GUIStyle m_BackgroundStyle = new GUIStyle();
    // 1x1 pixel texture used for fading
    private Texture2D m_FadeTexture;
    // default starting color: black and fully transparrent
    private Color m_CurrentScreenOverlayColor = new Color(0, 0, 0, 0);
    // default target color: black and fully transparrent
    private Color m_TargetScreenOverlayColor = new Color(0, 0, 0, 0);
    // the delta-color is basically the "speed / second" at which the current color should change
    private Color m_DeltaColor = new Color(0, 0, 0, 0);
    // make sure this texture is drawn on top of everything
    private int m_FadeGUIDepth = -1000;				

    private Color black = new Color(0, 0, 0, 1);
    private Color clear = new Color(0, 0, 0, 0);


    void Awake()
    {
        trackedObjLeft = controllerLeft.GetComponent<SteamVR_TrackedObject>();
        trackedObjRight = controllerRight.GetComponent<SteamVR_TrackedObject>();

        m_FadeTexture = new Texture2D(1, 1);
        m_BackgroundStyle.normal.background = m_FadeTexture;
        SetScreenOverlayColor(m_CurrentScreenOverlayColor);
    }

    /*
     * TODO: FIX THIS, somehow this causes the following Error
     * IndexOutOfRangeException: Array index is out of range.
     * SteamVR_Controller.Input (Int32 deviceIndex) (at Assets/SteamVR/Scripts/SteamVR_Controller.cs:151)
     * HapticFeedback.FixedUpdate () (at Assets/Scripts/HapticFeedback.cs:49)
     */
    void FixedUpdate () {
        //deviceLeft = SteamVR_Controller.Input(trackedObjLeft.index);
        //deviceRight = SteamVR_Controller.Input((int)trackedObjRight.index);

        if (proviceFeedback)
        {
            //deviceLeft.TriggerHapticPulse(200);
            //deviceRight.TriggerHapticPulse(200);
            SteamVR_Controller.Input((int)trackedObjRight.index).TriggerHapticPulse(200);
            SteamVR_Controller.Input((int)trackedObjLeft.index).TriggerHapticPulse(200);
        }
    }
    

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Debug.Log("Wall Collision");
            proviceFeedback = true;
            StartFade(black, 0.1f);
        }

        if (col.gameObject.tag == "ArtObject")
        {
            Debug.Log("ArtObject Collision");
            proviceFeedback = true;
            StartFade(black, 0.1f);
        }
    }


    void OnTriggerExit(Collider col)
    {
        proviceFeedback = false;
        StartFade(clear, 0.1f);
    }


    // draw the texture and perform the fade:
    private void OnGUI()
    {
        // if the current color of the screen is not equal to the desired color: keep fading!
        if (m_CurrentScreenOverlayColor != m_TargetScreenOverlayColor)
        {
            // if the difference between the current alpha and the desired alpha is smaller than delta-alpha * deltaTime, then we're pretty much done fading:
            if (Mathf.Abs(m_CurrentScreenOverlayColor.a - m_TargetScreenOverlayColor.a) < Mathf.Abs(m_DeltaColor.a) * Time.deltaTime)
            {
                m_CurrentScreenOverlayColor = m_TargetScreenOverlayColor;
                SetScreenOverlayColor(m_CurrentScreenOverlayColor);
                m_DeltaColor = new Color(0, 0, 0, 0);
            }
            else
            {
                // fade!
                SetScreenOverlayColor(m_CurrentScreenOverlayColor + m_DeltaColor * Time.deltaTime);
            }
        }

        // only draw the texture when the alpha value is greater than 0:
        if (m_CurrentScreenOverlayColor.a > 0)
        {
            GUI.depth = m_FadeGUIDepth;
            GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), m_FadeTexture, m_BackgroundStyle);
        }
    }

    // instantly set the current color of the screen-texture to "newScreenOverlayColor"
    // can be usefull if you want to start a scene fully black and then fade to opague
    public void SetScreenOverlayColor(Color newScreenOverlayColor)
    {
        m_CurrentScreenOverlayColor = newScreenOverlayColor;
        m_FadeTexture.SetPixel(0, 0, m_CurrentScreenOverlayColor);
        m_FadeTexture.Apply();
    }


    // initiate a fade from the current screen color (set using "SetScreenOverlayColor") towards "newScreenOverlayColor" taking "fadeDuration" seconds
    public void StartFade(Color newScreenOverlayColor, float fadeDuration)
    {
        if (fadeDuration <= 0.0f)       // can't have a fade last -2455.05 seconds!
        {
            SetScreenOverlayColor(newScreenOverlayColor);
        }
        else                    // initiate the fade: set the target-color and the delta-color
        {
            m_TargetScreenOverlayColor = newScreenOverlayColor;
            m_DeltaColor = (m_TargetScreenOverlayColor - m_CurrentScreenOverlayColor) / fadeDuration;
        }
    }
}


