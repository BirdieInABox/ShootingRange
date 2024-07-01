//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour, IEventListener
{
    private void Start()
    {
        //Add this as listener to event system
        EventManager.MainStatic.AddListener(this);
    }

    //On being hit
    public void Hit()
    {
        //Start the game via "GameStarted" event
        EventManager.MainStatic.FireEvent(new EventData(EventTypes.GameStarted));
        //Deactivate the visible/hittable part of the button
        transform.GetChild(0).gameObject.SetActive(false);
    }

    /// <summary>
    /// Called upon EventSystem sending an event
    /// </summary>
    /// <param name="receivedEvent">the received event, including type and content</param>
    public void OnEventReceived(EventData receivedEvent)
    {
        //If received event is of type "GameEnded"
        if (receivedEvent.Type == EventTypes.GameEnded)
        {
            //reactive the visible/hittable part
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
