using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFade : MonoBehaviour {

    public Animator Animation;
    public Object Portal;
    public Object Player;
    public bool ButtonPressed = false;
    public bool PortalEntered = false;
    private bool ChangeScene = false;
    private int LevelChange = 0;
    private int LevelToLoad;
    public bool BeginAnimation = false;
    public bool alreadyLoadedFisrtTime = false;
    public bool animationfinished=false;
    public bool FadeCompleted = false;

    void Update()
    {
        if(BeginAnimation == true && alreadyLoadedFisrtTime==false)
        {
            Animation.Play("FadeOutAnim");
            alreadyLoadedFisrtTime=true;
        }
        if (LevelChange == 0)
        {
            if (ButtonPressed)
            {
                ButtonPressed = false;
                ChangeScene = true;
            }
        }
        else if (LevelChange != 0)
        {
            if (PortalEntered)
            {
                PortalEntered = false;
                ChangeScene = true;
            }
        }
        if (ChangeScene)
        {
            LevelChange += 1;
            ChangeScene = false;
            FadeToNextLevel(LevelChange);
        }
    }
    public void AnimationDone()
    {
        animationfinished = true;
    }

    public void FadeToNextLevel(int LevelIndex)
    {
        Animation.Play("FadeAnimation");
        LevelToLoad = LevelIndex;
    }
    public void FadeComplete()
    {
        SceneManager.LoadScene(LevelToLoad);
        FadeCompleted = true;
        alreadyLoadedFisrtTime = false;
        BeginAnimation = true;
    }
	
}
