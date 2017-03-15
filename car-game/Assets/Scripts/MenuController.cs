using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public GameObject[] menuButtons;

    private int cursorPos = 0;
    private int cursorSpeedLimit = 10;
    private int currentCursorSpeedDecay = 0;
    private Color defColor;
    private Color selectColor;

    // Use this for initialization
    void Start () {
        defColor = menuButtons[0].GetComponent<UnityEngine.UI.Image>().color;
        selectColor = new Color(defColor.r, 255, defColor.b);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (currentCursorSpeedDecay <= 0)
        {
            if (Input.GetAxis("Vertical") > 0.25)
            {
                cursorPos = ((cursorPos - 1) % menuButtons.Length + menuButtons.Length)% menuButtons.Length;
                currentCursorSpeedDecay = cursorSpeedLimit;
            }
            else if (Input.GetAxis("Vertical") < -0.25)
            {
                cursorPos = ((cursorPos + 1) % menuButtons.Length + menuButtons.Length) % menuButtons.Length;
                currentCursorSpeedDecay = cursorSpeedLimit;
            }
        }
        else
        {
            currentCursorSpeedDecay--;
        }

        if (Input.GetButton("A"))
        {
            if(cursorPos == 0)
            {
                SceneManager.LoadScene("levelbuildtest");
            }
            if (cursorPos == 1)
            {
                Application.Quit();
            }
        }
    }

    void Update()
    {
        menuButtons[cursorPos].GetComponent<UnityEngine.UI.Image>().color = selectColor;
        for(int i = 0; i < menuButtons.Length; i++)
        {
            if (i != cursorPos)
            {
                menuButtons[i].GetComponent<UnityEngine.UI.Image>().color = defColor;
            }
        }
    }
}
