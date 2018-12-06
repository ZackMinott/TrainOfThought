using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))] //Automatically adds required component to gameObject
public class VGDAFade : MonoBehaviour {


    SpriteRenderer Renderer;
    public GameObject FadeAfter;
    bool Faded = false;
	// Use this for initialization
	void Start ()
    {
        Renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Renderer.color.a > 0 && FadeAfter != null? FadeAfter.GetComponent<VGDAFade>().GetFaded() : true)
        {
            Renderer.color = new Color(Renderer.color.r, Renderer.color.g, Renderer.color.b, Renderer.color.a - .005f);
        }
        if (Renderer.color.a <= 0)
        {
            Faded = true;
        }
	}

    public bool GetFaded()
    {
        return Faded;
    }
}
