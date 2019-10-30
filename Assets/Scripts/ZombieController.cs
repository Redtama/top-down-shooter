using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionHandler))]
public class ZombieController : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D rb;

    public RaycastHit2D[] hitResults = new RaycastHit2D[16];
    
    public float walkSpeed;
    public float runSpeed;    
    public float acquisitionRange;
    public float sightRange;    
    public float idleTime;   
    public float maxIdleWalkRange;
    public float minIdleWalkRange;

    private float skinWidth = 0.01f;
    private Vector2 movement;
    public Vector2 walkLocation;
    private bool trackingPlayer;
    public bool isIdle;
    public float startedIdling;
    private CollisionHandler collisionHandler;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collisionHandler = GetComponent<CollisionHandler>();
        isIdle = true;
        trackingPlayer = false;
        startedIdling = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        LookForPlayer();
        //check if zombie is already following the player
        if (trackingPlayer)
        {
            //if the player moves out of the zombies sight range, then make trackingPlayer false
            if (Vector2.Distance(player.position, transform.position) > sightRange)
            {
                trackingPlayer = false;
                SetNewLocation();
            }
            else
            {
                MoveToPlayer();
            }
        }
        else
        {
            //check if zombie is idle
            if (isIdle)
            {
                UpdateIdle();
            }
            else
            {
                MoveToLocation();
            }
        }
    }

    void MoveToPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        movement = direction * runSpeed * Time.deltaTime;
        collisionHandler.HandleCollisions(rb, ref movement, skinWidth);
        transform.Translate(movement, Space.World);
    }

    void UpdateIdle()
    {
        if (Time.time < startedIdling + idleTime)
        {
            IdleAnimate();
        }
        else
        {
            SetNewLocation();
            isIdle = false;
        }
    }

    void IdleAnimate()
    {
        //rotate a bit randomly
    }

    void SetNewLocation()
    {
        Vector2 travelDirection = Random.insideUnitCircle;
        int hitCount = rb.Cast(travelDirection, hitResults, maxIdleWalkRange);
        if (hitCount == 0)
        {
            Debug.Log("in first if");
            walkLocation = (Vector2)transform.position + travelDirection * Random.Range(minIdleWalkRange, maxIdleWalkRange);
        }
        else
        {
            for (int i = 0; i < hitCount; i++)
            {
                if (hitResults[i].distance < minIdleWalkRange)
                {
                    Debug.Log("in if");
                    SetNewLocation();
                }
                else
                {
                    Debug.Log("in else");
                    walkLocation = (Vector2)transform.position + travelDirection * Random.Range(minIdleWalkRange, hitResults[i].distance);
                }
            }
        }
    }

    void MoveToLocation()
    {
        Vector2 currentPos = transform.position;
        Vector2 newPos = Vector2.MoveTowards(currentPos, walkLocation, walkSpeed * Time.deltaTime);
        movement = newPos - currentPos;

        collisionHandler.HandleCollisions(rb, ref movement, skinWidth);
        transform.Translate(movement, Space.World);
                
        if (currentPos == walkLocation)
        {
            isIdle = true;
            startedIdling = Time.time;
        }
    }

    void LookForPlayer()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= acquisitionRange)
        {            
            trackingPlayer = true;
        }
    }
}
