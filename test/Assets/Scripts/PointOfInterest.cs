using UnityEngine;
using System.Collections;

public class PointOfInterest : MonoBehaviour {
   
    public float distance = 2.0f;
    public float speed = 3f;
    public GameObject billboard;
    public GameObject obj;
    public GameObject obj2;

    private float current_dist;
    private bool outside = true;


    public float duration = 3.0F;
    public Renderer rend;
    private Renderer rend2;





    void Start () {

        //rend = obj.GetComponent<Renderer>();
        //shader2 = Shader.Find("Diffuse");
        //shader = Shader.Find("LegacyShaders/Transparent/Diffuse");
        //rend.material.shader = shader;
        
        rend2 = obj2.GetComponent<Renderer>();
        rend2.material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        //Debug.Log("Shader obj: " + obj.GetComponent<Shader>().name);
        //Debug.Log("Shader obj2: " + obj2.GetComponent<Shader>().name);



    }

    // Update is called once per frame
    void Update () {

      
        // display billboard
        current_dist = Vector3.Distance(Camera.main.transform.position, this.transform.position);
        if (current_dist <= distance && outside)
        {
            outside = false;
            
            
            
            rend2 = obj2.GetComponent<Renderer>();
            rend2.material.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            FadeIn(speed);

            /*
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            rend.material.color = Color.Lerp(colorStart, colorEnd, lerp);
            */

        }
        if (!outside && current_dist > distance)
        {
            outside = true;
            
            rend2 = obj2.GetComponent<Renderer>();
            rend2.material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            FadeOut();
        }



        // fade billboard in & out
        /*
        //rend = blinder.GetComponent<Renderer>();
        for (float t = 0.0f; t < fadeTime; t += Time.deltaTime)
        {
            rend.material.color = new Color(0.0f, 0.0f, 0.0f, maxalpha * t / fadeTime);
            Debug.Log(maxalpha * t / fadeTime);
            //rend.material.color.a = Mathf.Lerp(1.0f, 0.0f, t / fadeTime);
        }

        rend.material.color = new Color(0.0f, 0.0f, 0.0f, maxalpha);
        new WaitForSeconds(2);
        rend.material.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        */

    }
    void FadeIn(float fadeTime)
    {
        billboard.SetActive(true);
        // do something
        Debug.Log("xxx");
        
        rend = obj.GetComponent<Renderer>();
        rend.material.color = new Color(1.0f, 0.0f, 0.0f, 0.6f);
        new WaitForSeconds(100);
        Debug.Log("wait1");
        rend = obj.GetComponent<Renderer>();
        rend.material.color = new Color(1.0f, 1.0f, 0.0f, 0.8f);
        new WaitForSeconds(100);
        Debug.Log("wait2");

        rend = obj.GetComponent<Renderer>();
        rend.material.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        /*
        for (float t = 0.0f; t < fadeTime; t += Time.deltaTime)
        {
            rend.material.color = new Color(0.0f, 0.0f, 0.0f, t / fadeTime);
            Debug.Log(t / fadeTime);
            //rend.material.color.a = Mathf.Lerp(1.0f, 0.0f, t / fadeTime);
        }
        */
    }

    void FadeOut()
    {
        billboard.SetActive(false);
        rend = obj.GetComponent<Renderer>();
        rend.material.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    }

}
