using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[Header("Normal Attributes")]
	public float normalSpeed;
	public float normalJumpVelocity;
	[Header("Shadow Attributes")]
	public float shadowSpeed;
	public float shadowJumpVelocity;
	public GameObject norm;
	public GameObject shadow;
	[HideInInspector]
	public bool isNormalForm = true;

    private bool isGrounded = true;
	private bool inLight = false;
	private SpriteRenderer spriteRenderer;

	Rigidbody2D rb2d;

	// Use this for initialization
	void Awake () {
		//Instantiate (norm, new Vector2(this.transform.position.x, this.transform.position.y));
		rb2d = GetComponent<Rigidbody2D> ();		
	}

    // For checking ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
    }

	//for checking light, allows to turn into shadow
	private void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "lightsource") {
			inLight = true;
		}
	}

	//for exiting light
	private void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "lightsource") {
			inLight = false; 
			if (!isNormalForm) //switches back to normal when leaving light source
				PlayerSwitch ();
		}
	}

    // Update is called once per frame
    void Update () {
		//movement
		float movement = Input.GetAxis ("Horizontal");

		if (isNormalForm) {
			rb2d.velocity = new Vector2 (movement * normalSpeed, rb2d.velocity.y); //Normal Speed
			Jump (normalJumpVelocity); //normal jump
			if(Input.GetKeyDown(KeyCode.E))
				PlayerSwitch();
		} else if (isNormalForm == false) {
			if (isGrounded)
				rb2d.velocity = new Vector2 (movement * shadowSpeed, rb2d.velocity.y); //Shadow Speed
			else if (!isGrounded)
				rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
			Jump(shadowJumpVelocity); //shadow jump
			if(Input.GetKeyDown(KeyCode.E))
				PlayerSwitch();
		}


	}

	//Game Object change
	void PlayerSwitch(){
		if (isNormalForm && inLight) {
			isNormalForm = false;
			norm.SetActive (false);
			shadow.SetActive (true);
		} else if (isNormalForm == false) {
			isNormalForm = true;
			shadow.SetActive (false);
			norm.SetActive (true);
		}
	}

	/*
	//Sprite Change Function
	void ChangeSprite(){
		if (spriteRenderer.sprite == norm.GetComponent<SpriteRenderer> ().sprite) {
			spriteRenderer.sprite = shadow.GetComponent<SpriteRenderer> ().sprite; //sets sprite image to shadow
			speed *= speedMultiplier; //sets speed to prefab
			jumpVelocity *= jumpMultiplier; //sets jump height to prefab
		}
        else {
			speed /= speedMultiplier; 
			jumpVelocity /= jumpMultiplier;
		}
	}
	*/

	//Jumping Mechanic
	void Jump(float jumpVelocity){
		if (Input.GetKeyDown (KeyCode.Space) && isGrounded) {
			rb2d.AddForce (transform.up * jumpVelocity);
			isGrounded = false;
		}
	}



}
