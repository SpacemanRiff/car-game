using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public GameObject[] menuButtons;
    public GameObject[] carChoices;

    private int cursorPos = 0;
    private int cursorSpeedLimit = 10;
    private int currentCursorSpeedDecay = 0;
    private Color defColor;
    private Color selectColor;
    private bool inCarSelect = false;
    private int carCursorPos = 0;

    // Use this for initialization
    void Start () {
        defColor = menuButtons[0].GetComponent<UnityEngine.UI.Image>().color;
        selectColor = new Color(defColor.r, 255, defColor.b);
        SetCarButtonsActive(false);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (currentCursorSpeedDecay <= 0)
        {
            if (Input.GetAxis("Vertical") > 0.25)
            {
                if (!inCarSelect)
                {
                    cursorPos = ((cursorPos + 1) % menuButtons.Length + menuButtons.Length) % menuButtons.Length;
                    currentCursorSpeedDecay = cursorSpeedLimit;
                }
                else
                {
                    carCursorPos = ((carCursorPos + 1) % carChoices.Length + carChoices.Length) % carChoices.Length;
                    currentCursorSpeedDecay = cursorSpeedLimit;
                }
            }
            else if (Input.GetAxis("Vertical") < -0.25)
            {
                if (!inCarSelect)
                {
                    cursorPos = ((cursorPos - 1) % menuButtons.Length + menuButtons.Length) % menuButtons.Length;
                    currentCursorSpeedDecay = cursorSpeedLimit;
                }
                else
                {
                    carCursorPos = ((carCursorPos - 1) % carChoices.Length + carChoices.Length) % carChoices.Length;
                    currentCursorSpeedDecay = cursorSpeedLimit;
                }
            }
        }
        else
        {
            currentCursorSpeedDecay--;
        }

        if (!inCarSelect)
        {
            if (Input.GetButton("A"))
            {
                if (cursorPos == 0)
                {
                    PlayerPrefs.SetInt("selectedCar", carCursorPos);
                    SceneManager.LoadScene("levelbuildtest");
                }
                if (cursorPos == 1 && currentCursorSpeedDecay <= 0)
                {
                    inCarSelect = true;
                    currentCursorSpeedDecay = cursorSpeedLimit;
                    SetCarButtonsActive(true);
                }
                if (cursorPos == menuButtons.Length - 1)
                {
                    Application.Quit();
                }
            }
            if (Input.GetAxis("Horizontal") > 0.25)
            {
                if (cursorPos == 1)
                {
                    inCarSelect = true;
                    SetCarButtonsActive(true);
                }
            }
        }
        else
        {
            if (Input.GetButton("A") || Input.GetButton("B") || Input.GetAxis("Horizontal") < -0.25)
            {
                if (inCarSelect && currentCursorSpeedDecay <= 0)
                {
                    inCarSelect = false;
                    currentCursorSpeedDecay = cursorSpeedLimit;
                    SetCarButtonsActive(false);
                }
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
        carChoices[carCursorPos].GetComponent<UnityEngine.UI.Image>().color = selectColor;
        for (int i = 0; i < carChoices.Length; i++)
        {
            if (i != carCursorPos)
            {
                carChoices[i].GetComponent<UnityEngine.UI.Image>().color = defColor;
            }
        }
    }

    void SetCarButtonsActive(bool b)
    {
        for (int i = 0; i < carChoices.Length; i++)
        {
            carChoices[i].SetActive(b);
        }
    }
}
