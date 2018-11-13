using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public float ShakeDuration = 0.3f;  //Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f; //Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f; //Cinemachine Noise Proile Parameter

    private float ShakeElapsedTime = 0f;

    //Cinemachine Shake
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public Player1 player;

    private void Start()
    {
        player = player.GetComponent<Player1>();

        //Get the Virtual Camera noise profile
        if(VirtualCamera != null)
        {
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }

    private void Update()
    {

        //If the Cinemachine component is not set, avoid update
        if(VirtualCamera != null && virtualCameraNoise != null)
        {
            //If Camera Shake effect is still playing 
            if(ShakeElapsedTime > 0)
            {
                //Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                //Update Shake Timer
                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                //If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                ShakeElapsedTime = 0f;
            }
        }
    }

    public void initiateShake()
    {
        ShakeElapsedTime = ShakeDuration;
    }
}
