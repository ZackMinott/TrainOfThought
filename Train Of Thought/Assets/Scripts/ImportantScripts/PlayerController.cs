using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Collider2D))] //adds boxCollider2D to gameObject
public class PlayerController : MonoBehaviour {
    public LayerMask collisionMask; //determines which objects we want to collide with

    const float skinWidth = .015f; 
    [Header("Rays Being Fired")]
    public int horizontalRayCount = 4; //Amount of rays being fired horizontally
    public int verticalRayCount = 4; //Amount of rays being fired vertically

    float maxClimbAngle = 60; //maximum angle that can be climbed
    float maxDescendAngle = 60;

    //Calculates spacing between each ray
    float horizontalRaySpacing;
    float verticalRaySpacing;

    Collider2D collider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    private void Start()
    {
        collider = GetComponent<Collider2D>();

        CalculateRaySpacing();
    }

    //Moves player
    public void Move(Vector3 velocity, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;

        if(velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);

        if (standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    void HorizontalCollisions(ref Vector3 velocity) //ref: any change to velocity made in this method will change the velocity in the method the variable was referenced from
    {
        float directionX = Mathf.Sign(velocity.x); //Get direction of y velocity
        float rayLength = Mathf.Abs(velocity.x) + skinWidth; //grabs length of the ray

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight; //if moving down set raycast origins to bottomleft and if moving up set raycast origins to top left
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            //if raycast hits something
            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up); //grabs the angle of the incline for moving up SLOPES
                if(i == 0 && slopeAngle <= maxClimbAngle)
                {
                    //This is solely for maintaining velocity when descending slopes 
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    //if starting to climb a new slope
                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth; 
                        velocity.x -= distanceToSlopeStart * directionX; //when we call climb slope method it only uses the velocity x that it has once it actually reaches the slope
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX; //adds back the velocity
                }

                //runs this only if the player is not climbing a slope or if the slope Angle is too large
                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX; //c
                    rayLength = hit.distance; //calculates distance from obstacle

                    //This chunk of code keeps the player from bouncing around when colliding with an object on a slope
                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = directionX == -1; //if hit something and collision is moving left, then collisions.left is true
                    collisions.right = directionX == 1; //same as above
                }
            }
        }
    }


    void VerticalCollisions(ref Vector3 velocity) //ref: any change to velocity made in this method will change the velocity in the method the variable was referenced from
    {
        float directionY = Mathf.Sign(velocity.y); //Get direction of y velocity
        float rayLength = Mathf.Abs(velocity.y) + skinWidth; //grabs length of the ray

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = ( directionY == -1 ) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft; //if moving down set raycast origins to bottomleft and if moving up set raycast origins to top left
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
            //if raycast hits something
            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                //avoids jumbling around when colliding with an object above while climbing
                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }    
        }

        //This chunk of code allows the player to move smoothly at changes in slope angles
        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    //Allows to smoothly climb up slopes
    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        //Checks to see if player is jumping 
        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true; //used to treat angles as touching ground
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }

    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up); //hit.normal is a direction that is perpendicular to the slope
            if(slopeAngle !=0 && slopeAngle <= maxDescendAngle)
            {
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds; //gets bounds of collider
        bounds.Expand(skinWidth * -2); //shrinks the bounds on all sides by the skinWidth

        //Sets all the bounds of the collider
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds; //gets bounds of collider
        bounds.Expand(skinWidth * -2); //shrinks the bounds on all sides by the skinWidth

        //sets a minimum of at least two rays being fired in both directions
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue); 
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1); //spacing would be entire length of the bounds if min value
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    //holds data that will not be modified later
    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        //resets all bools to false
        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
        }
    }
}
