using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed;
	public float jumpVelocity;

	//public Sprite normalBoy;
	//public Sprite shadowBoy;
	public GameObject norm;
	public GameObject shadow;

	private float speedMultiplier = 1.5f;
	private float jumpMultiplier = 2.5f;
	private bool grounded = false;
	private bool inLight = false;
	private SpriteRenderer spriteRenderer;

	Rigidbody2D rb2d;

	// Use this for initialization
	void Awake () {
		rb2d = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		if (spriteRenderer.sprite == null)
			spriteRenderer.sprite = norm.GetComponent<SpriteRenderer> ().sprite;
	}
	
	// Update is called once per frame
	void Update () {
		//movement
		float movement = Input.GetAxis ("Horizontal");
		rb2d.velocity = new Vector2(movement * speed, rb2d.velocity.y);


		//Jump
		if (Input.GetKeyDown (KeyCode.Space)) {
			rb2d.AddForce (transform.up * jumpVelocity);
		}

		//Change Sprite
		if (Input.GetKeyDown (KeyCode.E)) {
			ChangeSprite ();
		}

	}

	//Sprite Change Function
	void ChangeSprite(){
		if (spriteRenderer.sprite == norm.GetComponent<SpriteRenderer> ().sprite) {
			spriteRenderer.sprite = shadow.GetComponent<SpriteRenderer> ().sprite; //sets sprite image to shadow
			speed *= speedMultiplier; //sets speed to prefab
			jumpVelocity *= jumpMultiplier; //sets jump height to prefab
		} else {
			spriteRenderer.sprite = norm.GetComponent<SpriteRenderer> ().sprite; //sets sprite image to normal boy
			speed /= speedMultiplier; 
			jumpVelocity /= jumpMultiplier;
		}
	}



}
