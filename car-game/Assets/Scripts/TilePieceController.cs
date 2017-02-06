using UnityEngine;
using System.Collections;

public class TilePieceController : MonoBehaviour {

    public GameObject spawnPoint;

    private Vector3 destinationVector;
    private bool isSpawnTile;
    private bool isBeingPlaced;
    private bool placable = true;

	// Use this for initialization
	void Start () {
        destinationVector = transform.position;
        isBeingPlaced = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = Vector3.Lerp(transform.position, destinationVector, Time.deltaTime * 10);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        if (other.tag == "TilePiece")
        {
            placable = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "TilePiece")
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

    public void SetBeingPlaced(bool isBeingPlaced)
    {
        this.isBeingPlaced = isBeingPlaced;
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
