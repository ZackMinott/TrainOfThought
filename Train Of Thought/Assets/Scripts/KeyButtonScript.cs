using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButtonScript : MonoBehaviour {

    public bool pressDirectionPositive = true; //the direction the button acts as if pressed from, positive on the axis or negative on the axis
    public bool isXButton = false; //the axis the ubtton operates on, True for X Axis, false for Y Axis
    public float distance = 1.0f; //how far the button travels when pressed
    private float proximity = 1.4f; //how close you need to be to press the button
    public KeyCode keybind = KeyCode.T;
    public GameObject player;
    private bool isPushed = false; //whether the button has been pushed

    // Use this for initialization
    private void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        if (Input.GetKeyDown(keybind) && Mathf.Abs(Vector3.Distance(transform.position, player.transform.position)) < proximity && player.activeSelf)
        {
            if (pressDirectionPositive && isXButton)
            {
               transform.Translate(new Vector3(-distance, 0));
               isPushed = true;
            }
            else if (!pressDirectionPositive && isXButton)
            {
                    transform.Translate(new Vector3(distance, 0));
                    isPushed = true;
            }
            else if (pressDirectionPositive && !isXButton)
            {
                    transform.Translate(new Vector3(0, -distance));
                    isPushed = true;
            }
            else if (!pressDirectionPositive && !isXButton)
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
