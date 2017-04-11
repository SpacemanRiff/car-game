using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class BuilderController : MonoBehaviour {

    public float CameraSpeed = 1;
    public float smoothTime = 10f;
    public Text tileOutput;
    public Text debugOutput;
    public GameObject timer;
    private int tilesPlaced = 0;
    private GameObject tilePiece;
    private GameObject spawnTile;
    private List<string> trackPieces;
    private int trackListIndex;

	// Use this for initialization
	void Start () {
        trackPieces = new List<string>();
        AddToList( "StraightAway", "Corner", "T-Piece", "Bendy", "Bridge");
        trackListIndex = 0;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + Vector3.back * CameraSpeed * Input.GetAxis("Vertical");
        transform.position = transform.position + Vector3.right * CameraSpeed * Input.GetAxis("Horizontal");
        if(!trackPieces.Contains("Finish") && tilesPlaced > 1)
        {
            AddToList("Finish");
        }
        if (Input.GetButtonDown("A"))
        {
            if (tilePiece == null)
            {
                if (spawnTile == null)
                {
                    tilePiece = this.CreateTilePiece("StartPiece");
                    Debug.Log("created");

                }
                else
                {
                    tilePiece = this.CreateTilePiece(trackPieces[trackListIndex % trackPieces.Count]);
                    Debug.Log("created");
                }
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
            if (tilePiece != null && spawnTile != null)
            {
                Destroy(tilePiece);
                trackListIndex += 1;
                tilePiece = this.CreateTilePiece(trackPieces[trackListIndex % trackPieces.Count]);

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
                GameObject car = new GameObject();
                ButtonController.CarType carType = (ButtonController.CarType)PlayerPrefs.GetInt("selectedCar") ;
                switch (carType)
                {
                    case ButtonController.CarType.CAR:
                        car = (GameObject)Instantiate(Resources.Load("Car"));
                        break;
                    case ButtonController.CarType.VAN:
                        car = (GameObject)Instantiate(Resources.Load("Van"));
                        break;
                    default:
                        car = (GameObject)Instantiate(Resources.Load("Car"));
                        break;
                }
                car.transform.position = spawnTile.GetComponent<TilePieceController>().GetSpawnLocation().position;
                car.transform.rotation = spawnTile.GetComponent<TilePieceController>().GetSpawnLocation().rotation;
                timer.GetComponent<TimerController>().StartTimer();
                Destroy(gameObject);
            }
        }


        if (tilePiece != null)
        {
            tilePiece.GetComponent<TilePieceController>().SetDestinationVector(new Vector3(Mathf.Floor(transform.position.x / 50) * 50, 0.1f, Mathf.Floor(transform.position.z / 50) * 50));
        }
        tileOutput.text = trackPieces[trackListIndex % trackPieces.Count];

        setInputDebugText();
        if (Input.GetButtonDown("Exit"))
        {
            SceneManager.LoadScene("mainmenu");
        }
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

    private void AddToList(params string[] parameters)
    {
        for (int i = 0; i < parameters.Length; i++)
        {
            trackPieces.Add(parameters[i]);
        }
    }
}
