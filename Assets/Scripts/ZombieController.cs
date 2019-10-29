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

    // Start is called before the first frame updatewa
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
        movement = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(player.position.x, player.position.y), runSpeed * Time.deltaTime);
        transform.position = movement;
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
        movement = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walkLocation, walkSpeed * Time.deltaTime);
        transform.position = movement;
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

    void CheckCollision()
    {
        float castDistance = movement.magnitude + skinWidth;

        int hitCount = rb.Cast(movement.normalized, hitResults, castDistance);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = hitResults[i];

            Vector2 slopeNormal = hit.normal;
            Vector2 slopePathTest = Vector2.Perpendicular(hit.normal);
            if (Vector2.Dot(movement, slopePathTest) > 0)
            {
                movement = slopePathTest * Vector2.Dot(movement.normalized, slopePathTest) * movement.magnitude;
            }
            else
            {
                movement = -slopePathTest * Vector2.Dot(movement.normalized, -slopePathTest) * movement.magnitude;
            }
        }
    }
}
