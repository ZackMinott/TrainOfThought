using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MovieControl : MonoBehaviour {

    GameObject camera;
    public GameObject playAfter;
    public GameObject screenBlock;
    VideoPlayer videoPlayer;
    bool played = false;

    // Use this for initialization
    void Start ()
    {
        camera = GameObject.Find("Main Camera");
        videoPlayer = camera.GetComponent<UnityEngine.Video.VideoPlayer>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (playAfter != null && playAfter.GetComponent<VGDAFade>().GetFaded() && !played)
        {
            videoPlayer.Play();
            played = true;
            
        }
        if (!videoPlayer.isPlaying && played && videoPlayer.targetCameraAlpha > 0)
        {
            GameObject.Find("Main menu").GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            videoPlayer.targetCameraAlpha -= .01f;
            screenBlock.GetComponent<SpriteRenderer>().color = new Color(screenBlock.GetComponent<SpriteRenderer>().color.r, screenBlock.GetComponent<SpriteRenderer>().color.g, screenBlock.GetComponent<SpriteRenderer>().color.b, 0);
        }
    }
}
