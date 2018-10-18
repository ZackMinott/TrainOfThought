using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float speed = 5.0f;
	public float jumpForce = 550.0f;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		//movement
		float movement = Input.GetAxis ("Horizontal");
		rb.velocity = new Vector3 (movement * speed, rb.velocity.y, rb.velocity.z);

		//Jump
		if (Input.GetKeyDown (KeyCode.Space)) {
			rb.AddForce (transform.up * jumpForce);
		}
	}
}
