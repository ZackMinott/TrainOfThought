using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] //adds boxCollider2D to gameObject
[RequireComponent(typeof(Rigidbody2D))] //adds rigidbody2D to gameObject
public class PlatformMover : MonoBehaviour
{

    public LayerMask passengerMask;
    public LayerMask collisionMask; //determines which objects we want to collide with

    public Vector3[] localWaypoints; //waypoints that are displayed for visualization of paltform destinations
    Vector3[] globalWaypoints; //waypoint used to move the platforms

    const float skinWidth = .015f;

    public float speed;
    public bool cyclic;
    public float waitTime;
    [Range(0, 3)]
    public float easeAmount;

    bool movementActive = true; //determines if the platform will move
    public bool activateOnTouch = false; //platform activates when touched
    public bool buttonActivated = false; //platform activates when a button is pressed
    public bool moveOnce = false; //platform only moves once
    public bool moveAfterOtherPlatform; //platform moves after another platform has moved
    bool notMoved = true;
    public GameObject button;
    public GameObject platform;

    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;

    [Header("Rays Being Fired")]
    public int horizontalRayCount = 4; //Amount of rays being fired horizontally
    public int verticalRayCount = 4; //Amount of rays being fired vertically

    //Calculates spacing between each ray
    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    Rigidbody2D rigidbody;
    RaycastOrigins raycastOrigins;

    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, PlayerController> passengerDictionary = new Dictionary<Transform, PlayerController>();

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;

        if (activateOnTouch || buttonActivated || moveAfterOtherPlatform)
        {
            movementActive = false;
        }

        globalWaypoints = new Vector3[localWaypoints.Length]; //store all of the waypoints for use
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }

        CalculateRaySpacing();
    }

    public void Update()
    {
        if (buttonActivated)
        {
            if (button.GetComponent<KeyButtonScript>() != null ? button.GetComponent<KeyButtonScript>().getPressed() : button.GetComponent<ButtonScript>() != null ? button.GetComponent<ButtonScript>().getPressed() : false)
            {
                movementActive = true;
            }
        }
        if (moveAfterOtherPlatform)
        {
            if (!platform.GetComponent<PlatformMover>().getNotMoved())
            {
                movementActive = true;
            }
        }
        UpdateRaycastOrigins();
        if (movementActive && notMoved && ((platform != null)? !platform.GetComponent<PlatformMover>().getNotMoved() : true))
        {
            Vector3 velocity = CalculatePlatformMovement();

            CalculatePassengerMovement(velocity);

            //movement of the platform and passengers
            MovePassengers(true);
            transform.Translate(velocity);
            MovePassengers(false);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (activateOnTouch)
        {
            movementActive = true;
        }
    }

   public void OnCollisionExit2D(Collision2D collision)
    {
        if (activateOnTouch)
        {
            movementActive = false;
        }
    }

    public bool getNotMoved()
    {
        return notMoved;
    }

    float Ease(float x) //used for non-constant velocity of platofmr movement, easeAmount = 0 for constant velocity
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector3 CalculatePlatformMovement()
    {
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += (Time.deltaTime * speed) / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1) //determines if we have reached a waypoint
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;
            if (!cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints); //if we have reached the last waypoint, go back in reverse
                    if (moveOnce)
                    {
                        notMoved = false;
                    }
                }
            }
            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in passengerMovement) //move every passenger
        {
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<PlayerController>());
            }
            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //vertically moving platform
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth; //grabs length of the ray

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft; //if moving down set raycast origins to bottomleft and if moving up set raycast origins to top left
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit) //if the raycast hits something
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = (velocity.y - (hit.distance - skinWidth)) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        //horizontally moving platform
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth; //grabs length of the ray

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight; //if moving down set raycast origins to bottomleft and if moving up set raycast origins to top left
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit) //if the raycast hits something
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }
            }
        }

        // passenger is on top of horizontally or downward moving platform

        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2; //grabs length of the ray

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i); //if moving down set raycast origins to bottomleft and if moving up set raycast origins to top left

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit) //if the raycast hits something
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;
                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
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

    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

    private void OnDrawGizmos() //draws waypoints for visualization
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = 0.3f;
            for (int i = 0; i < localWaypoints.Length; i++)
            {
                //if the application is running, will display the waypoints being moved between
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }

}