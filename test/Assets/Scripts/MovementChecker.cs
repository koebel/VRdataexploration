using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovementChecker : MonoBehaviour {
    public GameObject player;
    public int interval = 2;
    private Vector3 prevPos;
    private Vector3 currentPos;
    private Vector3 startPos;
    private bool moving = true;
    private bool firstmove = false;

    private string startscene = "Pavillon1984";


    // Use this for initialization
    void Start () {
        //prevPos = player.transform.position;
        
        startPos = player.transform.position;
        currentPos = startPos;
        prevPos = startPos;

        StartCoroutine(CheckMovement(interval));

    }
	
	// Update is called once per frame
	void Update () {
        currentPos = player.transform.position;

        if (!moving)
        {
            Debug.Log("Restart Application!!!!!");
            SceneManager.LoadScene(startscene);
        }

    }


    IEnumerator CheckMovement(int time)
    {
        yield return new WaitForSeconds(time);
        //prevPos = currentPos;
        //startPos = player.transform.position;
        while (moving)
        {
            yield return new WaitForSeconds(time);
            Debug.Log(currentPos);

            
            if (currentPos != prevPos) 
            {
                Debug.Log("prev Position: " + prevPos + "  -----  new Position: " + currentPos);
                prevPos = currentPos;
              
                firstmove = true;
            }

            if (currentPos == prevPos)
            {
                Debug.Log("same Position!!!!");
                if (firstmove)
                {
                    Debug.Log("current Position: " + currentPos + "  -----  start Position: " + prevPos);
                    moving = false;
                }

            }
            

        }
    }
}
