using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BuilderController : MonoBehaviour {

    public float CameraSpeed = 1f;
    public float smoothTime = 10f;
    public Text tileOutput, debugOutput, FileLoaderText;
    public Image FileLoaderBox;
    public GameObject timer;

    private int tilesPlaced = 0;
    private int currentCursorSpeedDecay = 0;
    private int cursorSpeedLimit = 10;
    private int trackListIndex, fileCursor;
    private bool isSelectingFile;
    private string fileName;
    private GameObject tilePiece, spawnTile;
    private List<string> trackPieces, fileList;

    // Use this for initialization
    void Start ()
    {
        trackListIndex = 0;
        fileCursor = 0;
        trackPieces = new List<string>();
        AddToList("StraightAway", "Corner", "T-Piece", "Bendy", "Bridge");
        isSelectingFile = false;
        fileName = "LevelSaves/" + DateTime.Now.ToString("MMddyyyyhhmmss") + ".txt";
        setFileLoaderVisibility(false);
        PopulateFileList();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isSelectingFile)
        {
            transform.position = transform.position + Vector3.back * CameraSpeed * Input.GetAxis("Vertical");
            transform.position = transform.position + Vector3.right * CameraSpeed * Input.GetAxis("Horizontal");
            if (!trackPieces.Contains("Finish") && tilesPlaced > 1)
            {
                AddToList("Finish");
            }
            if (Input.GetButtonDown("A"))
            {
                if (tilePiece == null)
                {
                    tilePiece = spawnTile == null ? this.CreateTilePiece("StartPiece") : this.CreateTilePiece(trackPieces[trackListIndex % trackPieces.Count]);
                }
                else
                {
                    if (tilePiece.GetComponent<TilePieceController>().GetPlacable())
                    {
                        this.PlaceTile();
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
            if (Input.GetButtonDown("Y") && tilePiece != null && spawnTile != null)
            {
                Destroy(tilePiece);
                trackListIndex += 1;
                tilePiece = CreateTilePiece(trackPieces[trackListIndex % trackPieces.Count]);
            }
            if (Input.GetButtonDown("B") && tilePiece != null)
            {
                Destroy(tilePiece);
            }
            if (Input.GetButtonDown("Start") && spawnTile != null)
            {
                StartGame();
            }
            if (Input.GetButtonDown("FileList"))
            {
                isSelectingFile = true;
                setFileLoaderVisibility(true);
            }
        }
        else
        {
            if (currentCursorSpeedDecay <= 0)
            {
                if (Input.GetAxis("Vertical") > 0.25)
                {
                    fileCursor = ((fileCursor + 1) % fileList.Count + fileList.Count) % fileList.Count;
                    currentCursorSpeedDecay = cursorSpeedLimit;
                }
                else if (Input.GetAxis("Vertical") < -0.25)
                {
                    fileCursor = ((fileCursor - 1) % fileList.Count + fileList.Count) % fileList.Count;
                    currentCursorSpeedDecay = cursorSpeedLimit;
                }
            } else
            {
                currentCursorSpeedDecay--;
            }
            if (Input.GetButtonDown("FileList"))
            {
                isSelectingFile = false;
            }
            if (Input.GetButtonDown("Start"))
            {
                LoadFile(fileList.ElementAt(fileCursor));
                setFileLoaderVisibility(false);
                //StartGame();
                isSelectingFile = false;
                //StartGame();
            }
            setFileListText();

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

    private void setFileListText()
    {
        string outString = "";
        for(int i = (fileList.Count- 5 > 0) ? fileList.Count - 5 : 0; i < fileList.Count; i++){
            outString += (i == fileCursor) ? " > " : "   ";
            outString += fileList.ElementAt(i) + "\n";
        }
        FileLoaderText.text = outString;
    }

    private void setFileLoaderVisibility(bool b)
    {
        FileLoaderBox.enabled = b;
        FileLoaderText.enabled = b;
    }

    private GameObject CreateTilePiece(string pieceName)
    {
        GameObject piece = (GameObject)Instantiate(Resources.Load(pieceName), new Vector3(Mathf.Floor(transform.position.x / 50) * 50, 0, Mathf.Floor(transform.position.z / 50) * 50), new Quaternion(0, 0, 0, 0));
        piece.GetComponent<TilePieceController>().SetBeingPlaced(true);
        piece.GetComponent<TilePieceController>().SetPlaceID(tilesPlaced);
        piece.GetComponent<TilePieceController>().SetName(pieceName);
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
        //tilePiece.GetComponent<TilePieceController>().SetDestinationVector(new Vector3(Mathf.Floor(transform.position.x / 50) * 50, 0, Mathf.Floor(transform.position.z / 50) * 50));
        tilePiece.GetComponent<TilePieceController>().SetBeingPlaced(false);
        //AddTrackPieceToFile(tilesPlaced, Mathf.Floor(transform.position.x / 50) * 50, Mathf.Floor(transform.position.z / 50) * 50, tilePiece.GetComponent<TilePieceController>().GetRotation(),tilePiece.GetComponent<TilePieceController>().GetName());
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

    private void PopulateFileList()
    {
        DirectoryInfo dir = new DirectoryInfo("LevelSaves/");
        FileInfo[] info = dir.GetFiles("*.*");
        fileList = new List<string>();
        foreach (FileInfo f in info)
        {
            fileList.Add(f.ToString().Substring(f.ToString().Length - 18, 18));
            Debug.Log(fileList.Last<string>());
        }
    }

    private void AddTrackPieceToFile(int pieceNumber, float x, float z, float rot, string pieceName)
    {
        using (StreamWriter sw = File.AppendText(fileName))
        {
            sw.WriteLine("{0} {1} {2} {3} {4}", pieceNumber, x, z, rot, pieceName);
        }
    }

    private void LoadFile(string fileName)
    {
        string line;
        StreamReader file = new StreamReader("LevelSaves\\" + fileName);
        if (tilePiece != null)
        {
            Destroy(tilePiece);
        }
        while ((line = file.ReadLine()) != null)
        {
            string[] words = line.Split(' ');
            tilePiece = CreateTilePiece(words[4]);
            tilePiece.transform.position = new Vector3(Convert.ToSingle(words[1]), 0f, Convert.ToSingle(words[2]));
            tilePiece.GetComponent<TilePieceController>().SetRotation(Convert.ToSingle(words[3]));
            tilePiece.GetComponent<TilePieceController>().SetPlaceID(Convert.ToInt32(words[0]));
            if(words[4] == "StartPiece")
            {
                spawnTile = tilePiece;
                tilePiece.GetComponent<TilePieceController>().SetSpawnTile();
            }
            PlaceTile();
        }
    }

    private void StartGame()
    {
        if (tilePiece != null)
        {
            Destroy(tilePiece);
        }
        GameObject car = new GameObject();
        ButtonController.CarType carType = (ButtonController.CarType)PlayerPrefs.GetInt("selectedCar");
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
