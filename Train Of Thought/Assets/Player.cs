using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed;
	public float jumpVelocity = 20f;

	private bool grounded = false;

	Rigidbody2D rb2d;

	// Use this for initialization
	void Awake () {
		rb2d = GetComponent<Rigidbody2D> ();
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

	}
}
