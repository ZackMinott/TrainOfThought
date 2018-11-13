using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGlassWall : MonoBehaviour {

    Player1 player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player1>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.inGlass = true;
        Debug.Log("InGlass: " + player.inGlass);
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.inGlass = false;
        }
    }
}
