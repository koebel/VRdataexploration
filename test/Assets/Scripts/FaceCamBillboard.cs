using UnityEngine;
using System.Collections;

public class FaceCamBillboard : MonoBehaviour {

    public Camera Camera;

    void Update()

/*    {
        transform.LookAt(transform.position + Camera.transform.rotation * Vector3.forward,
            Camera.transform.rotation * Vector3.up);
    }
    */

 {
        Vector3 v = Camera.transform.position - transform.position;
       v.x = v.z = 0.0f;
        transform.LookAt(Camera.transform.position - v); 
    }
}
