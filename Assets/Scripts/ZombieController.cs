using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionHandler))]
public class ZombieController : MonoBehaviour
{
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
    private Transform player;

    HealthHandler health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<HealthHandler>();
        rb = GetComponent<Rigidbody2D>();
        collisionHandler = GetComponent<CollisionHandler>();
        isIdle = true;
        trackingPlayer = false;
        startedIdling = Time.time;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.health > 0)
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
    }

    void MoveToPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;        
        movement = direction * runSpeed * Time.deltaTime;
        if (movement != Vector2.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }        
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
 
    }

    void SetNewLocation()
    {
        Vector2 travelDirection = Random.insideUnitCircle;
        walkLocation = (Vector2)transform.position + travelDirection * Random.Range(minIdleWalkRange, maxIdleWalkRange);
    }

    void SetNewLocationOld()
    {
        Vector2 travelDirection = Random.insideUnitCircle;
        int hitCount = rb.Cast(travelDirection, hitResults, maxIdleWalkRange);
        if (hitCount == 0)
        {
            walkLocation = (Vector2)transform.position + travelDirection * Random.Range(minIdleWalkRange, maxIdleWalkRange);
        }
        else
        {
            for (int i = 0; i < hitCount; i++)
            {
                if (hitResults[i].distance < minIdleWalkRange)
                {
                    SetNewLocation();
                }
                else
                {
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
        if (movement != Vector2.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(newPos.x - currentPos.x, newPos.y - currentPos.y, 0));
        }        
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
