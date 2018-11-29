using UnityEngine;
using System.Collections;

public class SetStartposition : MonoBehaviour {

    public GameObject player;
    public static Vector3 position = new Vector3(0.0f, 0.0f, 16.0f);
    public static Vector3 defaultposition = new Vector3(0.0f, 0.0f, 16.0f);
    public static int currentYear = 0;

    // Use this for initialization
    void Start () {
        player.transform.position = position;
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
