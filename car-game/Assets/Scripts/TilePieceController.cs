using UnityEngine;
using System.Collections;

public class TilePieceController : MonoBehaviour {

    public GameObject spawnPoint;
    public GameObject[] connectors;

    private Rigidbody rb;
    private Vector3 destinationVector;
    private bool isSpawnTile;
    private bool isBeingPlaced = false;
    private bool placable = true;
    private bool collidingWithPieces = false;
    private bool IsLastTile = false;
    private int ID = -1;
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
        CheckPlacable();
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
            collidingWithPieces = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "TrackPiece")
        {
            Debug.Log("exit");
            collidingWithPieces = false;
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

    public void SetLastTile(bool lastTile)
    {
        IsLastTile = lastTile;
    }

    public void SetBeingPlaced(bool beingPlaced)
    {
        isBeingPlaced = beingPlaced;
        Debug.Log("did the thing, " + isBeingPlaced);
    }

    public void SetPlaceID(int ID)
    {
        this.ID = ID;
    }

    public bool IsSpawnTile()
    {
        return isSpawnTile;
    }

    public bool IsLastTile()
    {
        return IsLastTile;
    }

    public Transform GetSpawnLocation()
    {
        return spawnPoint.transform;
    }

    public bool GetPlacable()
    {
        return placable;
    }

    public void Rotate(float angle)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + angle, transform.eulerAngles.z);
    }

    private void CheckPlacable()
    {
        placable = !collidingWithPieces && CheckConnections();
    }

    private bool CheckConnections()
    {
        bool isConnected = false;
        for (int i = 0; i < connectors.Length && !isConnected; i++)
        {
            isConnected = connectors[i].GetComponent<ConnectorController>().isConnected();
        }

        return isConnected || ID == 0;
    }
}
