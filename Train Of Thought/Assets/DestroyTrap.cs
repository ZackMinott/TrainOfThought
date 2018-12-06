using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrap : MonoBehaviour {
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
            StartCoroutine("destruction");
        }
    }

    IEnumerator destruction()
    {
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }
}
