using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [HideInInspector]
    public RaycastHit2D[] hitResults = new RaycastHit2D[16];

    private ContactFilter2D contactFilter;
    private bool collidedLastFrame = false;
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

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = hitResults[i];            
            Vector2 slopeParallel = Vector2.Perpendicular(hit.normal);
            Vector2 newDirection = Mathf.Sign(Vector2.Dot(movement, slopeParallel)) * slopeParallel;            
            
            // If we're already touching the slope then move along it.
            if (collidedLastFrame && (oldSlopeNormal != hit.normal) || hitCount == 1)
            {
                movement = newDirection * Vector2.Dot(movement.normalized, newDirection) * movement.magnitude;
                oldSlopeNormal = hit.normal;
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
