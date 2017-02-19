using UnityEngine;
using System.Collections;

public class TilePieceController : MonoBehaviour {

    public GameObject spawnPoint;

    private Rigidbody rb;
    private Vector3 destinationVector;
    private bool isSpawnTile;
    private bool isBeingPlaced = false;
    private bool placable = true;
    private Color defaultColor;
    private Color placableColor, unplacableColor;

	// Use this for initialization
	void Start () {
        destinationVector = transform.position;
        rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
        rb.useGravity = false;
        rb.isKinematic = true;
        defaultColor = gameObject.GetComponent<Renderer>().material.color;
        placableColor = new Color(defaultColor.r, 1, defaultColor.b);
        unplacableColor = new Color(1, defaultColor.g, defaultColor.b);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = Vector3.Lerp(transform.position, destinationVector, Time.deltaTime * 10);
    }

    void Update()
    {
        Debug.Log(isBeingPlaced);
        if (isBeingPlaced)
        {
            if (placable)
            {
                Debug.Log("should be green");
                gameObject.GetComponent<Renderer>().material.color = placableColor;
            }
            else
            {
                Debug.Log("should be red");
                gameObject.GetComponent<Renderer>().material.color = unplacableColor;
            }
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = defaultColor;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TrackPiece")
        {
            Debug.Log("enter");
            placable = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "TrackPiece")
        {
            Debug.Log("exit");
            placable = true;
        }
    }

    public void SetDestinationVector(Vector3 destinationVector)
    {
        this.destinationVector = destinationVector;
    }

    public void SetSpawnTile()
    {
        isSpawnTile = true;
    }

    public void SetBeingPlaced(bool beingPlaced)
    {
        isBeingPlaced = beingPlaced;
        Debug.Log("did the thing, " + isBeingPlaced);
    }

    public bool IsSpawnTile()
    {
        return isSpawnTile;
    }

    public Transform GetSpawnLocation()
    {
        return spawnPoint.transform;
    }

    public bool GetPlacable()
    {
        return spawnPoint.transform;
    }
}
