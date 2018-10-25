using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))] //Automatically adds required component to gameObject
public class Player1 : MonoBehaviour {
    [Header("Normal Atrributes")]
    public float normalJumpHeight = 4; //how high we want the character to jump
    public float normalTimeToJump = .4f; //how long character takes to reach highest point
    public float normalMoveSpeed = 6;
    [Header("Shadow Attributes")]
    public float shadowJumpHeight = 7;
    public float shadowTimeToJump = .6f;
    public float shadowMoveSpeed = 12;

    public GameObject norm;
    public GameObject shadow;

    [HideInInspector]
    public bool isNormalForm = true;
    private bool isGrounded = true;
    private bool inLight = false;

    float accelerationTimeAir = .2f; //time it takes to move while in the air
    float accelerationTimeGrounded = .1f; //time it takes to move/switch directions while grounded
    
    float normGravity;
    float shadowGravity;
    float normalJumpVelocity;
    float shadowJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    PlayerController controller;
    Rigidbody2D rb2d;

    private void Start()
    {
        controller = GetComponent<PlayerController>(); //grabs playerController component
        rb2d = GetComponent<Rigidbody2D>(); 

        //sets the gravity
        normGravity = (-2 * normalJumpHeight) / Mathf.Pow(normalTimeToJump, 2);
        shadowGravity = (-2 * shadowJumpHeight) / Mathf.Pow(shadowTimeToJump, 2);
        //sets the jump velocity by multiplying the time to jump by velocity
        normalJumpVelocity = Mathf.Abs(normGravity) * normalTimeToJump;
        shadowJumpVelocity = Mathf.Abs(shadowGravity) * shadowTimeToJump;
    }

    private void Update()
    {
        //sets y velocity to zero if colliding with any object
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0; //keeps gravity from accumulating when colliding with an object
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        

        float targetVelocityX = 0;
        //WORK WITH THIS
        if (isNormalForm)
        {
            targetVelocityX = input.x * normalMoveSpeed;
            Jump(normalJumpVelocity); //normal jump
            if (Input.GetKeyDown(KeyCode.E))
                PlayerSwitch();
        }
        else if (isNormalForm == false)
        {
            targetVelocityX = input.x * shadowMoveSpeed;
            if(!controller.collisions.below)
                targetVelocityX = input.x * 0;
            Jump(shadowJumpVelocity); //shadow jump
            if (Input.GetKeyDown(KeyCode.E))
                PlayerSwitch();
        }
        //WORK WITH THIS

        
        //Smooth Damp gradually changes a value towards a desired goal over time
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)? accelerationTimeGrounded:0); //Smooths the players movement on switching directions
        if (isNormalForm)
        {
            velocity.y += normGravity * Time.deltaTime; //applies gravity to velocity
        } else if (!isNormalForm)
        {
            velocity.y += shadowGravity * Time.deltaTime; //applies gravity to velocity
        }
        
        controller.Move(velocity * Time.deltaTime);


    }

    void PlayerSwitch()
    {
        if (isNormalForm && inLight)
        {
            isNormalForm = false;
            norm.SetActive(false);
            shadow.SetActive(true);
        }
        else if (isNormalForm == false)
        {
            isNormalForm = true;
            shadow.SetActive(false);
            norm.SetActive(true);
        }
    }

    void Jump(float jumpVelocity)
    {
        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }
    }

    //for checking light, allows to turn into shadow
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "lightsource")
        {
            inLight = true;
        }
    }

    //for exiting light
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "lightsource")
        {
            inLight = false;
            if (!isNormalForm) //switches back to normal when leaving light source
                PlayerSwitch();
        }
    }

}
