using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    public bool pressDirectionPositive = true; //the direction the button needs to be pressed from, positive on the axis or negative on the axis
    public bool isXButton = false; //the axis the ubtton operates on, True for X Axis, false for Y Axis
    public float distance = 1.0f; //how far the button travels when pressed
    [HideInInspector]public bool isPushed = false; //whether the button has been pushed

    // Use this for initialization
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (pressDirectionPositive && isXButton)
        {
            if (collision.transform.position.x > transform.position.x)
            {
                transform.Translate(new Vector3(-distance, 0));
                isPushed = true;
            }
        }
        else if (!pressDirectionPositive && isXButton)
        {
            if (collision.transform.position.x < transform.position.x)
            {
                transform.Translate(new Vector3(distance, 0));
                isPushed = true;
            }
        }
        else if (pressDirectionPositive && !isXButton)
        {
            if (collision.transform.position.y > transform.position.y)
            {
                transform.Translate(new Vector3(0, -distance));
                isPushed = true;
            }
        }
        else if (!pressDirectionPositive && !isXButton)
        {
            if (collision.transform.position.y < transform.position.y)
            {
                transform.Translate(new Vector3(0, distance));
                isPushed = true;
            }
        }
    }

    public bool getPressed()
    {
        return isPushed;
    }
}
