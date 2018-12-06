using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrap : MonoBehaviour {
    [SerializeField] GameObject button;

    ButtonScript buttonPressed;

    void Start()
    {
        buttonPressed = button.GetComponent<ButtonScript>(); 
    }

    void Update()
    {
        if(buttonPressed.isPushed == true)
        {
            Destroy(gameObject);
        }
    }
}
