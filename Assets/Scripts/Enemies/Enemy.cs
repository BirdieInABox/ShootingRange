//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEventListener
{
    [SerializeField]
    private float maxHealth;
    private float currentHealth;

    [SerializeField]
    private int points;

    //The time needed to respawn
    private float respawnTimer;

    [SerializeField] //The minimum amount of seconds for respawnTimer
    private float respawnMin = 5f;

    [SerializeField] //The maximum amount of seconds for respawnTimer
    private float respawnMax = 10f;

    [SerializeField] //start- and endpoint of a moving target
    private GameObject start,
        end;

    [SerializeField] //speed of target's movement
    private float speed;

    [SerializeField] //does target remove hitpoints or give hitpoints?
    private bool legalTarget;

    [SerializeField] //the visible and hittable part of the target
    private GameObject targetBody;

    //start and end positions
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Rigidbody myRigidbody;
    private Coroutine respawning; //the instance of the respawn coroutine

    private void Start()
    {
        //Add this as EventSystem listener
        EventManager.MainStatic.AddListener(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (myRigidbody.position == endPosition) //if platform is at end point
        {
            StartCoroutine(MovePlatform(gameObject, startPosition, speed)); //move to start point
        }
        else if (myRigidbody.position == startPosition) //if platform is at start point
        {
            StartCoroutine(MovePlatform(gameObject, endPosition, speed)); //move to end point
        }
    }

    private void Awake()
    {
        //get Rigidbody2D
        myRigidbody = GetComponent<Rigidbody>();
        //get positions
        startPosition = start.transform.position;
        endPosition = end.transform.position;
    }

    /// <summary>
    /// Moves platform from current position to pathDestination
    /// </summary>
    /// <param name="obj"> platform </param>
    /// <param name="pathDestination"> destination </param>
    /// <param name="speed">speed of platform </param>
    IEnumerator MovePlatform(GameObject obj, Vector3 pathDestination, float speed)
    {
        //get startPos from current position
        Vector3 startPosition = obj.transform.position;
        float time = 0f; //reset time

        //while platform hasn't arrived the destination
        while (myRigidbody.position != pathDestination)
        {
            //Lerp to destination at speed "speed"
            obj.transform.position = Vector3.Lerp(
                startPosition,
                pathDestination,
                (time / Vector3.Distance(startPosition, pathDestination)) * speed
            );
            time += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Called on enemy being hit
    /// </summary>
    /// <param name="damage"> the value the HP of the target is changed by </param>
    public void Hit(float damage)
    {
        //reduce current HP
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //on death, trigger destruction event
            DestroyTarget();
        }
    }

    /// <summary>
    /// Called on hp <= 0
    /// </summary>
    private void DestroyTarget()
    {
        //Destroy the visible/hittable part of the target
        Destroy(transform.GetChild(0).gameObject);
        //Start coroutine to respawn target, save instance
        respawning = StartCoroutine(RespawnTarget());
        //Send points gained/lost via event to GameManager
        EventManager.MainStatic.FireEvent(new EventData(EventTypes.TargetHit, points));
    }

    /// <summary>
    /// Called after destruction of target, respawns target at a delay
    /// </summary>
    IEnumerator RespawnTarget()
    {
        //set respawnTimer to a random value between min and max timer
        float respawnTimer = Random.Range(respawnMin, respawnMax);
        //Wait respawnTimer seconds
        yield return new WaitForSeconds(respawnTimer);
        //Respawn target
        SpawnTarget();
    }

    /// <summary>
    /// instantiates new clone with this as parent and resets HP
    /// </summary>
    private void SpawnTarget()
    {
        Instantiate(targetBody, this.transform.position, this.transform.rotation, this.transform);
        Reset();
    }

    /// <summary>
    /// Resets target's HP back to full
    /// </summary>
    private void Reset()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Called upon EventSystem sending an event
    /// </summary>
    /// <param name="receivedEvent">the received event, including type and content</param>
    public void OnEventReceived(EventData receivedEvent)
    {
        //If received event is of type "GameStarted"
        if (receivedEvent.Type == EventTypes.GameStarted)
        {
            //Spawn target and reset
            SpawnTarget();
        }
        //If received event is of type "GameEnded"
        else if (receivedEvent.Type == EventTypes.GameEnded)
        {
            //If a visible target is present as child, destroy it
            if (transform.childCount > 0)
                Destroy(transform.GetChild(0).gameObject);
            //If target is in the process of being respawned, cancel the instance of the coroutine
            if (respawning != null)
                StopCoroutine(respawning);
        }
    }
}
