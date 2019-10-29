﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    bool trackingPlayer = false;
    bool arrivedAtLocation = true;
    Vector2 travelLocation;
    Vector2 travelDirection;
    Vector2 movement;
    private RaycastHit2D[] hitResults = new RaycastHit2D[16];
    private Rigidbody2D rb;
    private float skinWidth = 0.01f;

    public GameObject player;
    public GameObject zombie;
    public float sightRange;
    public float speed;
    public float castRange;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LookForPlayer();
        if (trackingPlayer) TrackPlayer();
        else
        {
            if (arrivedAtLocation) SearchForLocation();
            else ZombieMove();
            Debug.Log(travelLocation);
        }
    }

    void IsSearching()
    {
        //zombie searching in radius for player
        if (arrivedAtLocation)
        {
            SearchForLocation();
        }
        ZombieMove();
    }

    void SearchForLocation()
    {          
        travelDirection = Random.insideUnitCircle;
        int hitCount = rb.Cast(travelDirection, hitResults, castRange);
        if (hitCount != 0)
        {
            travelLocation = travelDirection * hitResults[Random.Range(0, hitCount)].distance;
        }
        else
        {
            travelLocation = travelDirection * Random.Range(0, castRange);
        }
        arrivedAtLocation = false;
    }

    void TrackPlayer()
    {
        //some nasty pathfinding to player
    }

    void ZombieMove()
    {
        movement = travelDirection * speed * Time.fixedDeltaTime;
        if (movement.magnitude != 0)
        {
            TestCollisions();
            transform.Translate(movement, Space.World);
            if (Mathf.Abs(travelLocation.x - zombie.transform.position.x) < 0.05 || Mathf.Abs(travelLocation.y - zombie.transform.position.y) < 0.05)
            {
                arrivedAtLocation = true;
            }
        }
    }

    void LookForPlayer()
    {
        if (Vector2.Distance(player.transform.position, zombie.transform.position) <= sightRange)
        {
            trackingPlayer = true;
        }
    }

    void TestCollisions()
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
