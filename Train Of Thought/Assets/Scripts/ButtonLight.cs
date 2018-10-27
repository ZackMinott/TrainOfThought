using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLight : MonoBehaviour {

    public GameObject button;
    public GameObject lightSource;

	// Update is called once per frame
	void Update ()
    {
        if (button.GetComponent<ButtonScript>().getPressed())
        {
            lightSource.SetActive(true);
        }
	}
}
