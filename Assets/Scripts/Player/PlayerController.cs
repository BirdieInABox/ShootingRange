//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IEventListener
{
    public float walkSpeed = 5f; // walking speed
    public float sprintSpeed = 10f; //fast swimming speed
    private CharacterController controller;
    private Vector3 velocity;
    public float gravity = -9.81f; //The gravitational forces on the player
    private Vector3 direction;
    private Vector3 lookDirection;
    public GameObject bodyOfPlayer; //corporeal part of player
    private bool isSprinting = false;
    private PlayerInput inputActions;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        //Adds this as listener to the event system
        EventManager.MainStatic.AddListener(this);
    }

    private void Update()
    {
        //movement
        Walk();
    }

    /// /// <summary>
    /// Called upon EventSystem sending an event
    /// </summary>
    /// <param name="receivedEvent">the received event, including type and content</param>
    public void OnEventReceived(EventData receivedEvent)
    {
        //If received event is of type "GamePaused"
        if (receivedEvent.Type == EventTypes.GamePaused)
        {
            //Switch input maps
            SwitchInput();
        }
    }

    /// <summary>
    /// Called by Pause key, sends event "GamePaused"
    /// </summary>
    public void OnPause(InputValue value)
    {
        //Sends GamePaused event through the event system
        EventManager.MainStatic.FireEvent(new EventData(EventTypes.GamePaused));
    }

    /// <summary>
    /// Switches between UI and Game input maps
    /// This helps with blocking any illegal input during a paused game
    /// </summary>
    private void SwitchInput()
    {
        //If currently on UI map, switch to Game map
        if (inputActions.currentActionMap == inputActions.actions.FindActionMap("UI"))
        {
            inputActions.currentActionMap = inputActions.actions.FindActionMap("Game");
        }
        else //If currently on Game map, switch to UI map
        {
            inputActions.currentActionMap = inputActions.actions.FindActionMap("UI");
        }
    }

    /// <summary>
    /// Moves the CharacterController
    /// </summary>
    private void Walk()
    {
        //set direction to new vector3 using 2-axis movement, nullifying y-movement
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        //Get direction through camera direction
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        //Add sprint speed or walk speed, depending on status of isSPrinting
        moveDirection *= (isSprinting ? sprintSpeed : walkSpeed);
        //Calculate the direction to look towards
        lookDirection = moveDirection + bodyOfPlayer.transform.position;
        bodyOfPlayer.transform.LookAt(lookDirection);
        //Add manual gravity to moveDirection
        moveDirection.y += gravity;

        controller.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Called by Move keys (WASD, Arrows)
    /// </summary>
    public void OnMove(InputValue value)
    {
        //Get 2-axis movement values between -1 and 1
        direction = value.Get<Vector2>();
    }

    /// <summary>
    /// Called when OnSprint key is pressed down, called again on release
    /// </summary>
    public void OnSprint(InputValue value)
    {
        //toggle sprint status
        isSprinting = !isSprinting;
    }
}
