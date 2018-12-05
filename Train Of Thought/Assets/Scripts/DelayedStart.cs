using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedStart : MonoBehaviour
{
    public Player1 PlayerController;
    public GameObject FFStart;
    public GameObject FFFormBox;
   
    public GameObject FFEnd;
    public GameObject FFLight;

    public float initialDelay= 1f;
    public float controlReleaseDelay = 1f;
 
    // Use this for initialization
    private void Start ()
    {
        StartCoroutine(CoStartDelay());
       
    }

    private IEnumerator CoStartDelay()
    {
        PlayerController.enabled = false;
        yield return new WaitForSeconds(initialDelay); // Wait to turn on
        Debug.Log("waited");
        //PlayIdle();

        FFFormBox.SetActive(true);
        FFLight.SetActive(true);

        PlayerController.SetForm(false);

        bool done = false;
        while(!done)
        {
            // Basically Update
            if(Input.GetButtonDown("Horizontal"))
            {
                done = true;
            }
            yield return null;
        }

        // Remove Box
        FFFormBox.SetActive(false);
        FFLight.SetActive(false);

        FFEnd.SetActive(true);

        PlayerController.SetForm(true);
        yield return new WaitForSeconds(controlReleaseDelay);

        PlayerController.enabled = true;

    }
   
}
