using UnityEngine;
using System.Collections;


/*
	helper method to change the opacity of an object. 
    created on 19th july 2018 by k2

    IMPORTANT:
 	rendering mode of the shader of the to "obj" assigned game object shout be set to "Transparent" (Legacy Shaders/Transparent/Diffuse) in order to be able to manipulate the alpha value
*/


public class SetOpacity : MonoBehaviour {

    public GameObject obj;
    public float opacity = 0.7f;


    // Use this for initialization
    void Start()
    {
        var col = obj.GetComponent<Renderer>().material.color;

        // unity doesn't allow to directly access the alpha value because not all materials have an alpha
        col.a = opacity;
        obj.GetComponent<Renderer>().material.color = col;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
