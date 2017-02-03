using UnityEngine;
using System.Collections;

public class TilePieceController : MonoBehaviour {

    public GameObject spawnPoint;

    private Vector3 destinationVector;
    private bool isSpawnTile;

	// Use this for initialization
	void Start () {
        destinationVector = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, destinationVector, Time.deltaTime * 10);
    }

    public void SetDestinationVector(Vector3 destinationVector)
    {
        this.destinationVector = destinationVector;
    }

    public void SetSpawnTile()
    {
        isSpawnTile = true;
    }

    public bool IsSpawnTile()
    {
        return isSpawnTile;
    }

    public Transform GetSpawnLocation()
    {
        return spawnPoint.transform;
    }
}
