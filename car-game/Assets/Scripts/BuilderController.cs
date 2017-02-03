using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuilderController : MonoBehaviour {

    public float CameraSpeed = 1;
    public float smoothTime = 10f;
    public Text tileOutput;
    public Text debugOutput;
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
            tilePiece.transform.position = Vector3.Lerp(tilePiece.transform.position, new Vector3(Mathf.Floor(transform.position.x/50) * 50, 0, Mathf.Floor(transform.position.z / 50) * 50), Time.deltaTime * smoothTime);
        }
        tileOutput.text = trackPieces[trackListIndex % trackPieces.Length];

        setInputDebugText();
    }

    private void setInputDebugText()
    {
        debugOutput.text = "Horizontal: " + Input.GetAxis("Horizontal") + "\nVertical: " + Input.GetAxis("Vertical")  + "\nA: " + Input.GetButton("A") + "\nB: " + Input.GetButton("B") + "\nY: " + Input.GetButton("Y") + "\nLB: " + Input.GetButton("LB") + "\nRB: " + Input.GetButton("RB");
    }
}
