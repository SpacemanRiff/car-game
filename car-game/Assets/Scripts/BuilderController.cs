using UnityEngine;
using System.Collections;

public class BuilderController : MonoBehaviour {

    public float CameraSpeed = 1;
    private GameObject tilePiece;
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

            if (tilePiece == null) { 
                Debug.Log("created");
                trackListIndex = 0;
                tilePiece = (GameObject)Instantiate(Resources.Load(trackPieces[trackListIndex % trackPieces.Length]));
                tilePiece.transform.position = new Vector3(transform.position.x % 50, 0, transform.position.z % 50);
            }else
            {
                Debug.Log("placed");
                tilePiece = null;                
            }
        }
        if (Input.GetButtonDown("LB"))
        {
            if (tilePiece != null)
            {
                tilePiece.transform.eulerAngles = new Vector3(tilePiece.transform.eulerAngles.x, tilePiece.transform.eulerAngles.y - 90f, tilePiece.transform.eulerAngles.z);
            }
        }
        if (Input.GetButtonDown("RB"))
        {
            if (tilePiece != null)
            {
                tilePiece.transform.eulerAngles = new Vector3(tilePiece.transform.eulerAngles.x, tilePiece.transform.eulerAngles.y + 90f, tilePiece.transform.eulerAngles.z);
            }
        }
        if (Input.GetButtonDown("Y"))
        {
            if (tilePiece != null)
            {
                Destroy(tilePiece);
                trackListIndex += 1;
                tilePiece = (GameObject)Instantiate(Resources.Load(trackPieces[trackListIndex % trackPieces.Length]));

            }
        }
        if (Input.GetButtonDown("B"))
        {
            if (tilePiece != null)
            {
                Destroy(tilePiece);

            }
        }


        if (tilePiece != null)
        {
            tilePiece.transform.position = new Vector3(transform.position.x - (transform.position.x % 50), 0, transform.position.z - (transform.position.z % 50));
        }
    }
}
