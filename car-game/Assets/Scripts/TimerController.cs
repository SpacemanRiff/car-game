using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimerController : MonoBehaviour {

    public Text timerText;

    private float time = 0;
    private bool isRunning = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            time += Time.deltaTime;
        }
	}

    void FixedUpdate()
    {
        timerText.text = Math.Round(time,2) + " seconds";
    }

    public void StartTimer()
    {
        isRunning = true;
        time = 0;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
