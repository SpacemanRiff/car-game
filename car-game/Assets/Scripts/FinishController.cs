using UnityEngine;
using System.Collections;

public class FinishController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish Line")
        {
            Debug.Log("finished!");
            GameObject.Find("Timer").GetComponent<TimerController>().StopTimer();
        }
    }
}
