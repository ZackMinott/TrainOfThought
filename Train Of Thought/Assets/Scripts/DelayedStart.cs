using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedStart : MonoBehaviour {
    public GameObject PlayerController;
    public GameObject FFStart;
    public GameObject FFFormBox;
    public GameObject FFEnd;
    public GameObject FFLight;
    public GameObject Shadow;
    public GameObject Normal;
    public bool goPlayEnd = false;
 
    // Use this for initialization
    void Start () {
        StartCoroutine("StartDelay");
       
    }

    public void Update()
    {
        if (Input.GetButtonDown("Horizontal") && goPlayEnd == true)
        {
            FFFormBox.SetActive(false);
            FFEnd.SetActive(true);
            Normal.SetActive(true);
            Shadow.SetActive(false);
            FFLight.SetActive(false);
        }

    }

    IEnumerator StartDelay()
    {
        
        yield return new WaitForSeconds(1);
        Debug.Log("waited");
        PlayIdle();
        
    }
    public void PlayIdle()
    {
        FFFormBox.SetActive(true);
        Shadow.SetActive(true);
        Normal.SetActive(false);
        FFLight.SetActive(true);
        StartCoroutine("FFFormBoxDelay");
    }

    IEnumerator FFFormBoxDelay()
    {
        yield return new WaitForSeconds(1);
        PlayerController.GetComponent<Player1>().enabled = true;
        goPlayEnd = true;
    }
   
}
