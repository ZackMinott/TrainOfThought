using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))] //Automatically adds required component to gameObject
public class Player1 : MonoBehaviour {

    [Header("Animation Attributes")]
    public Animator My_AnimationNormal; // this takes an animation and can adjust what happens to said animation
    public Animator My_AnimationShadow;


    public double TimePassed; // this is in case we want to include time based scenarios
    public double TeleportCooldown = 1;
    public double Timeswitched;

    [Header("Normal Atrributes")]
    public float normalJumpHeight = 4; //how high we want the character to jump
    public float normalTimeToJump = .4f; //how long character takes to reach highest point
    public float normalMoveSpeed = 6;
    public float maxFall = 25; //max distance can fall before dying
    [Header("Shadow Attributes")]
    public float shadowJumpHeight = 7;
    public float shadowTimeToJump = .6f;
    public float shadowMoveSpeed = 12;

    [Header("Game Objects")]
    public GameObject norm;
    public GameObject shadow;
    public GameObject normParticles;
    public GameObject virtualCamera;
    public AudioSource switchSound;


    public bool isNormalForm = true;
    public bool isGrounded=false;
    public bool isSRunning;
    public bool isNRunning;
    public bool inLight = false;
    public bool is_Right = true; // needed to turn sprite left or right
    public bool changed;
    private int numbertimeschanged = 1;
    public bool inGlass = false;
    public bool playerCanMove = true; //will change back to false

    float accelerationTimeAir = .2f; //time it takes to move while in the air
    float accelerationTimeGrounded = .1f; //time it takes to move/switch directions while grounded
    
    float normGravity;
    float shadowGravity;
    float normalJumpVelocity;
    float shadowJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;
    float fallDistance; //used to detect death

    double NextTeleportTime;

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
        //if (isFrozen) return;

        TimePassed += Time.deltaTime;
        //sets y velocity to zero if colliding with any object
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0; //keeps gravity from accumulating when colliding with an object

        }
       
        if (controller.collisions.below)
        {
            isGrounded = true;
            if (fallDistance > maxFall)
            {
                Object.Destroy(this.transform.gameObject);
            }
            fallDistance = 0;
        }
        else
        {
            isGrounded = false;
        }
       
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float targetVelocityX = 0;

        /**
        * 
        * This is the Animation 
        * */
        if (!isNormalForm && changed && TimePassed - Timeswitched > .01)
        {
            changed = false;
            My_AnimationShadow.Play("SwitchToShadow", 0);
        }
        if (isNormalForm)
        {
            My_AnimationNormal.SetFloat("Running", Mathf.Abs(input.x * normalMoveSpeed)); // Sets the parameter for running so that if the speed is above 0 it will proceed with the running animation
            My_AnimationNormal.SetBool("Ground", isGrounded);
           
            My_AnimationNormal.SetBool("isRunning", isNRunning);
            if (Input.GetButton("Horizontal"))
            {

                isNRunning = true;
                //isSRunning = false;
            }
            else
            {
                isNRunning = false;
            }
        }
        else if(!isNormalForm)
        {
            My_AnimationShadow.SetFloat("Run", Mathf.Abs(input.x * shadowMoveSpeed));
            My_AnimationShadow.SetBool("Ground", isGrounded);
            
            My_AnimationShadow.SetBool("isRunning", isSRunning);

         
            if (Input.GetButton("Horizontal"))
            {

                isSRunning = true;
                //isSRunning = false;
            }
            else
            {
                isSRunning = false;
            }
        }
        if (is_Right)
        {
            if (input.x * normalMoveSpeed < 0 || input.x * shadowMoveSpeed < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                is_Right = false;
            }
        }
        else
        {
            if (input.x * normalMoveSpeed > 0 || input.x * shadowMoveSpeed > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                is_Right = true;
            }
        }

        //WORK WITH THIS
        if (isNormalForm)
        {
            targetVelocityX = input.x * normalMoveSpeed;
            Jump(normalJumpVelocity); //normal jump
            //shadow.SetActive(false);
            //norm.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && inLight)
            {   
                PlayerSwitch();
            }

        }
        else if (!isNormalForm)
        {
            targetVelocityX = input.x * shadowMoveSpeed;
            if (!controller.collisions.below)
                targetVelocityX = input.x * 0;
            Jump(shadowJumpVelocity); //shadow jump
            //norm.SetActive(false);
            //shadow.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && !inGlass)
            {
                PlayerSwitch();
            }
        }
        //WORK WITH THIS

  

        //Smooth Damp gradually changes a value towards a desired goal over time
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)? accelerationTimeGrounded:0); //Smooths the players movement on switching directions
        if (isNormalForm)
        {
            velocity.y += normGravity * Time.deltaTime; //applies gravity to velocity
        }
        else if (!isNormalForm)
        {
            velocity.y += shadowGravity * Time.deltaTime; //applies gravity to velocity
        }
        
        if(playerCanMove)
            controller.Move(velocity * Time.deltaTime);
        if (velocity.y < 0)
        {
            fallDistance -= velocity.y * Time.deltaTime;
        }

    }

    public void SetForm(bool isNormal)
    {
        if (isNormal == isNormalForm) return;

        changed = true;
        Timeswitched = TimePassed;

        isNormalForm = isNormal;
        norm.SetActive(isNormal);
        shadow.SetActive(!isNormal);
        normParticles.SetActive(isNormal);
        virtualCamera.GetComponent<CameraShake>().initiateShake();

        if (isNormal == false) // Set to Shadow
        {
            My_AnimationShadow.Play("SwitchToShadow", 0);
        }
        else
        {
            if (!inLight) // Still necessary?
                normParticles.SetActive(false);

            //My_AnimationShadow.SetTrigger("Change");
            My_AnimationNormal.Play("SwitchToPerson", 0);
        }
    }
    

    private void PlayerSwitch()
    {
        if (isNormalForm && inLight)
        {
            switchSound.Play();
            SetForm(false);
        }
        else if (isNormalForm == false)
        {
            switchSound.Play();
            SetForm(true);
        }
    }

    void Jump(float jumpVelocity)
    {
        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            
            velocity.y = jumpVelocity;
            if(isNormalForm == true)
            {
                My_AnimationNormal.Play("NJumpTakeOff");
            }
            else
            {
                My_AnimationShadow.Play("NJumpTakeOff");
            }
        }
    }

    //for checking light, allows to turn into shadow
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "lightsource")
        {
            inLight = true;
            if (isNormalForm)
            {
                normParticles.SetActive(true);
            }
        }

        if (col.gameObject.tag == "portal")
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (col.gameObject.tag == "transporter" && TimePassed > NextTeleportTime)
        {
           transform.position = col.GetComponent<TransportScript>().linkedTransporter.transform.position;
            NextTeleportTime = TimePassed + TeleportCooldown;
        }
    }


    //for exiting light
    private void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.tag == "lightsource")
        {
            inLight = false;
            normParticles.SetActive(false);
            if (!isNormalForm)
            {
                //switches back to normal when leaving light source
                PlayerSwitch();
            }
        }
    }

}
