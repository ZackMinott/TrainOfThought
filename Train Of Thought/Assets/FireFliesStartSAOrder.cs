using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFliesStartSAOrder : MonoBehaviour {
    public GameObject FireFliesStart;
	// Use this for initialization
	void Start () {
        StartCoroutine("FFStartTimer");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator FFStartTimer()
    {

        yield return new WaitForSeconds(1);
        Debug.Log("ff done");
    }
}
