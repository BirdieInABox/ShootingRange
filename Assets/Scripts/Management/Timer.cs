//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour, IEventListener
{
    //The end of the rotation in the circle
    private int maximum = 360;

    //The start of the rotation in the circle
    private int minimum = 0;

    //The calculated percentile of each integer
    private float percentile;

    //The time left in the current cycle
    public float timeLeft;

    //The duration of a cycle
    public float levelDuration = 30f;

    private Quaternion rotation;
    public bool levelInProgress = false;

    private Image handImg;
    private Color handColor;
    float currPercentile;

    // Update is called once per frame
    void Update()
    {
        if (levelInProgress && !Menu.gameIsPaused)
        {
            //The current rotation
            currPercentile = maximum - (timeLeft * percentile);
            //Passage of time
            timeLeft -= Time.deltaTime;
            //Rotating the game object
            gameObject.transform.rotation = Quaternion.Euler(0, 0, (float)minimum - currPercentile);

            //Automatic events
            //Is it after DayEnd?
            if (currPercentile >= maximum)
            {
                //End the day
                EndLevel();
            }
        }
    }

    private void Start()
    {
        //Add this as listener for the event system
        EventManager.MainStatic.AddListener(this);
        //get the image of the clock hand
        handImg = GetComponent<Image>();
        //get the original colour
        handColor = handImg.color;
    }

    void LateUpdate()
    {
        if (levelInProgress && !Menu.gameIsPaused)
        {
            //Update the clock hand's colour
            Color tempColour = handImg.color;
            //Reduce green value by percentage of rotation done
            tempColour.g = 1 - ((currPercentile / maximum) * 1);
            //increase red value respectively
            tempColour.r = ((currPercentile / maximum) * 1);
            handImg.color = tempColour;
        }
    }

    //When time runs out, end level
    public void EndLevel()
    {
        levelInProgress = false;
        EventManager.MainStatic.FireEvent(new EventData(EventTypes.GameEnded));
    }

    //start new level timer
    public void StartLevel()
    {
        handImg.color = handColor;
        //Calculate x/24 into degrees of a circle with 0 being minimum and 24 being maximum, in base config 0=0°, 24=360°
        percentile = ((float)maximum) / levelDuration;
        levelInProgress = true;

        //reset timer%
        timeLeft = levelDuration;
    }

    /// <summary>
    /// Called upon EventSystem sending an event
    /// </summary>
    /// <param name="receivedEvent">the received event, including type and content</param>
    public void OnEventReceived(EventData receivedEvent)
    {
        //If event received is of type "GameStarted"
        if (receivedEvent.Type == EventTypes.GameStarted)
        {
            //Begin timer
            StartLevel();
        }
    }
}
