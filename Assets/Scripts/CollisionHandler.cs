using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler
{
    public static RaycastHit2D[] HandleCollisions(Rigidbody2D rb, ref Vector2 movement, float skinWidth)
    {
        float castDistance = movement.magnitude + skinWidth;
        RaycastHit2D[] hitResults = new RaycastHit2D[16];

        int hitCount = rb.Cast(movement.normalized, hitResults, castDistance);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = hitResults[i];

            Vector2 slopeNormal = hit.normal;
            Vector2 slopeParallel = Vector2.Perpendicular(hit.normal);
            Vector2 newDirection = Mathf.Sign(Vector2.Dot(movement, slopeParallel)) * slopeParallel;
            
            movement = newDirection * Vector2.Dot(movement.normalized, newDirection) * movement.magnitude;
        }
        return hitResults;
    }
}
