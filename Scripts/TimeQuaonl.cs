using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeQuaonl : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    Text timeText;
    private void Start()
    {
        timeText = GetComponent<Text>();
        // Starts the timer automatically
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                gameObject.transform.parent.GetChild(2).gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
