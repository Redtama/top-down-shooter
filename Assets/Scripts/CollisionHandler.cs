using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [HideInInspector]
    public RaycastHit2D[] hitResults = new RaycastHit2D[16];

    private bool collidedLastFrame = false;    

    public void HandleCollisions(Rigidbody2D rb, ref Vector2 movement, float skinWidth)
    {
        float castDistance = movement.magnitude + skinWidth;
        RaycastHit2D[] hitResults = new RaycastHit2D[16];

        int hitCount = rb.Cast(movement.normalized, hitResults, castDistance);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = hitResults[i];            
            Vector2 slopeParallel = Vector2.Perpendicular(hit.normal);
            Vector2 newDirection = Mathf.Sign(Vector2.Dot(movement, slopeParallel)) * slopeParallel;
            
            // If we're already touching the slope then move along it.
            if (collidedLastFrame)
            {
                movement = newDirection * Vector2.Dot(movement.normalized, newDirection) * movement.magnitude;
            }
            // Otherwise close the distance to the slope.
            else
            {
                float distanceToSlope = hit.distance - skinWidth;
                movement = movement.normalized * distanceToSlope;
            }
        }

        collidedLastFrame = hitCount != 0;
    }
}
