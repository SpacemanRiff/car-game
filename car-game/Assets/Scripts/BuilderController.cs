using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuilderController : MonoBehaviour {

    public float CameraSpeed = 1;
    public float smoothTime = 10f;
    public Text tileOutput;
    public Text debugOutput;
    private int tilesPlaced = 0;
    private GameObject tilePiece;
    private GameObject spawnTile;
    private string[] trackPieces;
    private int trackListIndex;

	// Use this for initialization
	void Start () {
        trackPieces = new string[] { "StraightAway", "Corner", "T-Piece" };
        trackListIndex = 0;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + Vector3.back * CameraSpeed * Input.GetAxis("Vertical");
        transform.position = transform.position + Vector3.right * CameraSpeed * Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("A"))
        {

            if (tilePiece == null)
            {
                tilePiece = this.CreateTilePiece(trackPieces[trackListIndex % trackPieces.Length]);
                Debug.Log("created");
            }
            else
            {
                if (tilePiece.GetComponent<TilePieceController>().GetPlacable())
                {
                    this.PlaceTile();
                }
                else
                {
                    Debug.Log("can\'t sorry lol");
                }
            }
        }
        if (Input.GetButtonDown("LB"))
        {
            if (tilePiece != null)
            {
                tilePiece.GetComponent<TilePieceController>().Rotate(-90f);
            }
        }
        if (Input.GetButtonDown("RB"))
        {
            if (tilePiece != null)
            {
                tilePiece.GetComponent<TilePieceController>().Rotate(90f);
            }
        }
        if (Input.GetButtonDown("Y"))
        {
            if (tilePiece != null)
            {
                Destroy(tilePiece);
                trackListIndex += 1;
                tilePiece = this.CreateTilePiece(trackPieces[trackListIndex % trackPieces.Length]);

            }
        }
        if (Input.GetButtonDown("B"))
        {
            if (tilePiece != null)
            {
                Destroy(tilePiece);
            }
        }
        if (Input.GetButtonDown("Start"))
        {
            if (spawnTile != null)
            {
                if (tilePiece != null)
                {
                    Destroy(tilePiece);
                }
                GameObject car = (GameObject)Instantiate(Resources.Load("Car"));
                car.transform.position = spawnTile.GetComponent<TilePieceController>().GetSpawnLocation().position;
                car.transform.rotation = spawnTile.GetComponent<TilePieceController>().GetSpawnLocation().rotation;
                Destroy(gameObject);
            }
        }


        if (tilePiece != null)
        {
            tilePiece.GetComponent<TilePieceController>().SetDestinationVector(new Vector3(Mathf.Floor(transform.position.x / 50) * 50, 0.1f, Mathf.Floor(transform.position.z / 50) * 50));
        }
        tileOutput.text = trackPieces[trackListIndex % trackPieces.Length];

        setInputDebugText();
    }

    private void setInputDebugText()
    {
        debugOutput.text = "Horizontal: " + Input.GetAxis("Horizontal") + "\nVertical: " + Input.GetAxis("Vertical")  + "\nA: " + Input.GetButton("A") + "\nB: " + Input.GetButton("B") + "\nY: " + Input.GetButton("Y") + "\nLB: " + Input.GetButton("LB") + "\nRB: " + Input.GetButton("RB");
    }

    private GameObject CreateTilePiece(string pieceName)
    {
        GameObject piece = (GameObject)Instantiate(Resources.Load(pieceName), new Vector3(Mathf.Floor(transform.position.x / 50) * 50, 0, Mathf.Floor(transform.position.z / 50) * 50), new Quaternion(0, 0, 0, 0));
        piece.GetComponent<TilePieceController>().SetBeingPlaced(true);
        piece.GetComponent<TilePieceController>().SetPlaceID(tilesPlaced);
        return piece;
    }

    private void PlaceTile()
    {
        Debug.Log("placed");
        if (tilesPlaced <= 0)
        {
            tilePiece.GetComponent<TilePieceController>().SetSpawnTile();
            spawnTile = tilePiece;
            Debug.Log("spawn tile");
        }
        Debug.Log(tilePiece.GetComponent<TilePieceController>().IsSpawnTile());
        tilePiece.GetComponent<TilePieceController>().SetDestinationVector(new Vector3(Mathf.Floor(transform.position.x / 50) * 50, 0, Mathf.Floor(transform.position.z / 50) * 50));
        tilePiece.GetComponent<TilePieceController>().SetBeingPlaced(false);
        tilesPlaced++;
        tilePiece = null;
    }
}
