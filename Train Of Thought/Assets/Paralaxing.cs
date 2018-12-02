using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxing : MonoBehaviour
{
    [Header("GameObjects Background")]
    public GameObject Background1;
    public GameObject Background2;
    public GameObject Background3;
    public GameObject Background4;
    public GameObject Player;

    [HideInInspector]
    private float OriginalPosPlayer;
    private float UpdatedPosPlayer;
    private float CurrentPosPlayer;
    private bool playerMoved = false;

    //Moving player and time oriented stuff
    public bool MovingLeft = false;
    public bool MovingRight = false;
    private float PlayerInput;
    private float TimePassed = 0;
    private float CurrentTime;


    [Header("Moving Objects")]
    public float moveLimitObj1Left;
    public float moveLimitObj1Right;
    public float moveLimitObj2Left;
    public float moveLimitObj2Right;
    public float moveLimitObj3Left;
    public float moveLimitObj3Right;
    public float moveLimitObj4Left;
    public float moveLimitObj4Right;

    [Header("Object Movement speed")]
    public float moveSpeedObj1;
    public float moveSpeedObj2;
    public float moveSpeedObj3;
    public float moveSpeedObj4;

    [HideInInspector]
    private float Background1PosOrig;
    private float Background2PosOrig;
    private float Background3PosOrig;
    private float Background4PosOrig;

    [HideInInspector]
    private float Background1Pos;
    private float Background2Pos;
    private float Background3Pos;
    private float Background4Pos;


    private void Start()
    {
        //original position of backgrounds
        Background1PosOrig = Background1.transform.position.x;
        Background2PosOrig = Background2.transform.position.x;
        Background3PosOrig = Background3.transform.position.x;
        Background4PosOrig = Background4.transform.position.x;

        //player motion detection
        OriginalPosPlayer = Player.transform.position.x;
        CurrentPosPlayer = Player.transform.position.x;
        UpdatedPosPlayer = Player.transform.position.x;

        //Updated background positions (will be updated once detects player movement)
        Background1Pos = Background1.transform.position.x;
        Background2Pos = Background2.transform.position.x;
        Background3Pos = Background3.transform.position.x;
        Background4Pos = Background4.transform.position.x;
    }

    private void Update()
    {
        //time needed for player detection
        CurrentTime += Time.deltaTime;
        CurrentPosPlayer = Player.transform.position.x;
        //detects first player movement
        if ((CurrentPosPlayer - OriginalPosPlayer != 0 && playerMoved == false))
        {
            if (playerMoved == false)
            {
                playerMoved = true;
                UpdatedPosPlayer = Player.transform.position.x;
                PlayerInput = CurrentPosPlayer - UpdatedPosPlayer;
                TimePassed = CurrentTime;
            }
        }

        //player updated movement and what position to move background towards
        if (CurrentTime - TimePassed > .03 && playerMoved == true)
            {
                PlayerInput = CurrentPosPlayer - UpdatedPosPlayer;
                UpdatedPosPlayer = Player.transform.position.x;
                TimePassed = CurrentTime;
            }
        if (PlayerInput < 0)
        {
            MovingRight = false;
            MovingLeft = true;
        }
        else if (PlayerInput > 0)
        {
            MovingLeft = false;
            MovingRight = true;
        }
        else
        {
            MovingLeft = false;
            MovingRight = false;
        }


        if (MovingRight)
        {
            if (Background1Pos > Background1PosOrig - moveLimitObj1Left)
            {
                Background1.transform.position = new Vector3(Background1.transform.position.x - moveSpeedObj1, Background1.transform.position.y, Background1.transform.position.z);
                Background1Pos -= moveSpeedObj1;
            }
            if (Background2Pos > Background2PosOrig - moveLimitObj2Left)
            {
                Background2.transform.position = new Vector3(Background2.transform.position.x - moveSpeedObj2, Background2.transform.position.y, Background2.transform.position.z);
                Background2Pos -= moveSpeedObj2;
            }
            if (Background3Pos > Background3PosOrig - moveLimitObj3Left)
            {
                Background3.transform.position = new Vector3(Background3.transform.position.x - moveSpeedObj3, Background3.transform.position.y, Background3.transform.position.z);
                Background3Pos -= moveSpeedObj3;
            }
            if (Background4Pos > Background4PosOrig - moveLimitObj4Left)
            {
                Background4.transform.position = new Vector3(Background4.transform.position.x - moveSpeedObj4, Background4.transform.position.y, Background4.transform.position.z);
                Background4Pos -= moveSpeedObj4;
            }
        }
        else if (MovingLeft)
        {
            if (Background1Pos < Background1PosOrig + moveLimitObj1Left)
            {
                Background1.transform.position = new Vector3(Background1.transform.position.x + moveSpeedObj1, Background1.transform.position.y, Background1.transform.position.z);
                Background1Pos += moveSpeedObj1;
            }
            if (Background2Pos < Background2PosOrig + moveLimitObj2Left)
            {
                Background2.transform.position = new Vector3(Background2.transform.position.x + moveSpeedObj2, Background2.transform.position.y, Background2.transform.position.z);
                Background2Pos += moveSpeedObj2;
            }
            if (Background3Pos < Background3PosOrig + moveLimitObj3Left)
            {
                Background3.transform.position = new Vector3(Background3.transform.position.x + moveSpeedObj3, Background3.transform.position.y, Background3.transform.position.z);
                Background3Pos += moveSpeedObj3;
            }
            if (Background4Pos < Background4PosOrig + moveLimitObj4Left)
            {
                Background4.transform.position = new Vector3(Background4.transform.position.x + moveSpeedObj4, Background4.transform.position.y, Background4.transform.position.z);
                Background4Pos += moveSpeedObj4;
            }
        }
    }
    
}