using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class TeleportFade : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    public GameObject blinder;
    public float fadeTime = 3.0f;

    public Renderer rend;
    private float alpha = 0f;
    private float maxalpha = 1.0f;

    //http://wiki.unity3d.com/index.php/FadeObjectInOut

    // Use this for initialization
    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        rend = blinder.GetComponent<Renderer>();
        rend.material.color = new Color(0.0f, 0.0f, 0.0f, alpha);     
    }

    // Update is called once per frame
    void FixedUpdate () {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("You have released the Touchpad");
            /*
            //rend = blinder.GetComponent<Renderer>();
            for (float t = 0.0f; t < fadeTime; t += Time.deltaTime)
            {
                rend.material.color = new Color(0.0f, 0.0f, 0.0f, maxalpha * t / fadeTime);
                Debug.Log(maxalpha * t / fadeTime);
                //rend.material.color.a = Mathf.Lerp(1.0f, 0.0f, t / fadeTime);
            }
            */
            rend.material.color = new Color(0.0f, 0.0f, 0.0f, maxalpha);
            new WaitForSeconds(2);
            rend.material.color = new Color(0.0f, 0.0f, 0.0f, alpha);

        }

    }
}
