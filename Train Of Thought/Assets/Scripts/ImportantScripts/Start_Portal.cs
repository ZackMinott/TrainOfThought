using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Portal : MonoBehaviour {
    Animation anim;
    AnimationClip portal;

	// Use this for initialization
	void Start () {
        StartCoroutine(PlayPortal());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator PlayPortal()
    {
        anim.Play();

        yield return new WaitForSeconds(portal.length);
    }
}
