using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveLevelChanger : MonoBehaviour {

    public LevelFade LevelFade;
    public PlayGame PlayGame;
    public GameObject LevelFading;

    private bool ActiveLevelFade = true;

    private void Start()
    {
        LevelFade.BeginAnimation = true;
    }
    // Update is called once per frame
    void Update () {
        bool buttonPressed = PlayGame.GameStart;
        bool AnimationDone = LevelFade.animationfinished;
        if (LevelFade.FadeCompleted)
        {
            PlayGame.GameStart = false;
        }
        if (AnimationDone)
        {
            AnimationFinished();
        }
        if (buttonPressed)
        {
            ButtonPressed();
        }
        if (ActiveLevelFade)
        {
            ActiveLevelFade = false;
            LevelFade.animationfinished = false;
            LevelFading.SetActive(true);
        }
	}
    public void ButtonPressed()
    {
        ActiveLevelFade = true;
    }
    public void AnimationFinished()
    {
        LevelFading.SetActive(false);
    }
    
}
