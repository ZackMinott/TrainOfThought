using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))] //Automatically adds required component to gameObject
public class Player1 : MonoBehaviour {
    public float jumpHeight = 4; //how high we want the character to jump
    //ALTER TIME TO JUMP FOR SHADOW
    public float timeToJump = .4f; //how long character takes to reach highest point
    float accelerationTimeAir = .2f; //time it takes to move while in the air
    float accelerationTimeGrounded = .1f; //time it takes to move/switch directions while grounded
    //ALTER MOVE SPEED FOR SHADOW
    public float moveSpeed = 6;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>(); //grabs playerController component

        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJump;
    }

    private void Update()
    {
        //sets y velocity to zero if colliding with any object
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0; //keeps gravity from accumulating when colliding with an object
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        //Smooth Damp gradually changes a value towards a desired goal over time
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)? accelerationTimeGrounded:accelerationTimeAir); //Smooths the players movement on switching directions
        velocity.y += gravity * Time.deltaTime; //applies gravity to velocity
        controller.Move(velocity * Time.deltaTime);
    }

}
