using UnityEngine;
using System.Collections;

public class ConnectorController : MonoBehaviour {

    private bool connected;

	// Use this for initialization
	void Start () {
        connected = false;
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Connector")
        {
            Debug.Log("connected");
            connected = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Connector")
        {
            Debug.Log("disconnected");
            connected = false;
        }
    }

    public bool isConnected()
    {
        return connected;
    }
}
