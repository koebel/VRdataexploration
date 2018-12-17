using UnityEngine;
using System.Collections;

public class FaceCamBillboard : MonoBehaviour {

    public Camera Camera;

    void Update()
    {
        Vector3 v = Camera.transform.position - transform.position;

        //lock to rotation around y axis
        //v.x = v.z = 0.0f;
        //transform.LookAt(Camera.transform.position - v);

        // allow rotation in all directions
        transform.LookAt(Camera.transform.position);
    }
}
