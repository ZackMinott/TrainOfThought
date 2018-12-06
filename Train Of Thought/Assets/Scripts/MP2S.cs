using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP2S : MonoBehaviour
{

    float dirY, moveSpeed = 1f;
    bool moveRight = true;

    // Update is called once per frame
    void FixedUpdate()   
    {
        if (transform.position.y > 4f)
            moveRight = false;
        if (transform.position.y< -4f)
            moveRight = true;

        if (moveRight)
            transform.position = new Vector2(transform.position.x,transform.position.y + moveSpeed * Time.deltaTime);
        else
            transform.position = new Vector2(transform.position.x,transform.position.y - moveSpeed * Time.deltaTime) ;
    }
}
