using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* from https://stackoverflow.com/questions/13708395/how-can-i-draw-a-circle-in-unity3d */


public class CircleMaker : MonoBehaviour {

    public GameObject marker;
    public float thetaScale = 0.01f;        //Set lower to add more points
    public int size; //Total number of points in circle
    public float radius = 3f;
    public float linewidth = 0.2f;
    public Material mat;
    private LineRenderer lineRenderer;
    

    // Use this for initialization
    void Start () {
		
	}

    // 
    void Awake()
    {
        float sizeValue = (2.0f * Mathf.PI) / thetaScale;
        size = (int) sizeValue;
        size++;
        lineRenderer = marker.AddComponent<LineRenderer>();
        lineRenderer.material = mat;
        lineRenderer.SetWidth(linewidth, linewidth); //thickness of line
        lineRenderer.SetVertexCount(size);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos;
        float theta = 0f;
        for (int i = 0; i < size; i++)
        {
            theta += (2.0f * Mathf.PI * thetaScale);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            x += gameObject.transform.position.x;
            y += gameObject.transform.position.y;
            pos = new Vector3(x, 0, y);
            lineRenderer.SetPosition(i, pos);   
        }
    }
}
