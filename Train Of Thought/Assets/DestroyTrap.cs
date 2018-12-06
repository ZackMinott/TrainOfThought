using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrap : MonoBehaviour {
    [SerializeField] GameObject button;
    [SerializeField] float secondsToDestroy;

    ButtonScript buttonPressed;

    void Start()
    {
        if (button == null) return;
        buttonPressed = button.GetComponent<ButtonScript>();
    }

    void Update()
    {
        if (buttonPressed == null) return;
        if (buttonPressed.isPushed == true)
        {
            StartCoroutine("destruction");
        }
    }

    IEnumerator destruction()
    {
        yield return new WaitForSeconds(secondsToDestroy);
        Destroy(gameObject);
    }
}
