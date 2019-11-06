using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [HideInInspector]
    public RaycastHit2D[] hitResults = new RaycastHit2D[16];

    private ContactFilter2D contactFilter;
    private int contactsLastFrame = 0;
    private Vector2 oldSlopeNormal = Vector2.zero;

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    public void HandleCollisions(Rigidbody2D rb, ref Vector2 movement, float skinWidth)
    {
        float castDistance = movement.magnitude + skinWidth;

        int hitCount = rb.Cast(movement.normalized, contactFilter, hitResults, castDistance);     

        if (hitCount == 1)
        {
            RaycastHit2D hit = hitResults[0];
            Vector2 slopeParallel = Vector2.Perpendicular(hit.normal);
            Vector2 newDirection = Mathf.Sign(Vector2.Dot(movement, slopeParallel)) * slopeParallel;

            // If we're already touching the slope then move along it.
            if (contactsLastFrame > 0)
            {
                movement = newDirection * Vector2.Dot(movement.normalized, newDirection) * movement.magnitude;
            }
            // Otherwise, close the distance to the slope.
            else
            {
                float distanceToSlope = hit.distance - skinWidth;
                movement = movement.normalized * distanceToSlope;
            }
        }

        if (hitCount == 2)
        {
            RaycastHit2D hit1 = hitResults[0];
            RaycastHit2D hit2 = hitResults[1];

            // If we're already in the corner then work out what to do next.
            if (contactsLastFrame == 2)
            {

            }
            // Otherwise, close the distance to the corner.
            else
            {
                
            }

            float angleBetweenHits = Vector2.Angle(hit1.normal, hit2.normal);


        }

        contactsLastFrame = hitCount;
    }
}
