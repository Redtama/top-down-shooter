using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D rb;

    private RaycastHit2D[] hitResults = new RaycastHit2D[16];
    
    public float walkSpeed;
    public float runSpeed;    
    public float aquisitionRange;
    public float sightRange;    
    public float idleTime;   
    public float castRange;
    public float minRange;
    public float skinWidth;

    public Vector2 movement;
    public Vector2 walkLocation;
    public bool trackingPlayer;
    public bool isIdle;
    public float startedIdling;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
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
            }
            else
            {
                MoveToPlayer();
            }
        }

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

    void MoveToPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        movement = direction * runSpeed * Time.deltaTime;
        CollisionHandler.HandleCollisions(rb, ref movement, skinWidth);
        transform.Translate(movement, Space.World);
    }

    void UpdateIdle()
    {
        if (Time.time > startedIdling + idleTime)
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
        int hitCount = rb.Cast(travelDirection, hitResults, castRange);
        if (hitCount == 0)
        {
            walkLocation = travelDirection * Random.Range(minRange, castRange);
        }
        else
        {
            walkLocation = travelDirection * hitResults[Random.Range(Mathf.RoundToInt(minRange), hitCount)].distance;
        }
    }

    void MoveToLocation()
    {
        Vector2 direction = (walkLocation - new Vector2(transform.position.x, transform.position.y)).normalized;
        movement = direction * walkSpeed * Time.deltaTime;
        CollisionHandler.HandleCollisions(rb, ref movement, skinWidth);
        transform.Translate(movement, Space.World);
        if (transform.position.x == walkLocation.x && transform.position.y == walkLocation.y)
        {
            isIdle = true;
            startedIdling = Time.time;
        }
    }

    void LookForPlayer()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= aquisitionRange)
        {
            Debug.Log(Vector2.Distance(player.transform.position, transform.position));
            trackingPlayer = true;
        }
    }
}
