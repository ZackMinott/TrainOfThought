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

    public bool isNormalForm = true;
    private bool isGround = true;
	public bool inLight;
	private SpriteRenderer spriteRenderer;

	Rigidbody2D rb2d;

	// Use this for initialization
	void Awake () {
		rb2d = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		if (spriteRenderer.sprite == null)
			spriteRenderer.sprite = shadow.GetComponent<SpriteRenderer> ().sprite;
	}

    // For checking ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGround = true;
        }
    }

    //For checking light
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "lightsource")
        {
          inLight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "lightsource")
        {
            isNormalForm = true;
            inLight = false;
        }
    }




    void Update () {
        if (isNormalForm)
        {
            //normal movement
            float normalMovement = Input.GetAxis("Horizontal");
            transform.position = transform.position + new Vector3(normalMovement * normalSpeed * Time.deltaTime, 0, 0);


            //normal jump
            if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
            {
                rb2d.AddForce(new Vector2(0, normalJumpVelocity), ForceMode2D.Impulse);
                isGround = false;
            }
            //switching to specific area only
            if (Input.GetKeyDown(KeyCode.E) && inLight == true)
            {
                isNormalForm = false;
            }


        }
        else
        {
            //shadow movement
            if (isGround == true)
            {
            float shadowMovement = Input.GetAxis("Horizontal");
            transform.position = transform.position + new Vector3(shadowMovement * shadowSpeed * Time.deltaTime, 0, 0);
            }
            //shadow jump
            if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
            {
                rb2d.AddForce(new Vector2(0, shadowJumpVelocity), ForceMode2D.Impulse);
                isGround = false;
            }
            //switching to specific area only
            if (Input.GetKeyDown(KeyCode.E))
            {
                isNormalForm = true;
            }

        }
	}

}
