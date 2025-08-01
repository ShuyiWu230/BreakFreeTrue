using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool shouldRotate;
    public float WaitingTime,StayTime;
    private float WaitingTimer,StayTimer;
    // Start is called before the first frame update
    void Start()
    {
        WaitingTimer = WaitingTime;
        StayTimer = StayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldRotate == false && WaitingTimer >= 0) 
        {
            WaitingTimer -= Time.deltaTime;
        }
        
        if (WaitingTimer < 0) 
        {
            shouldRotate = true;
            WaitingTimer = WaitingTime;
        }

        if (shouldRotate == true) 
        {
            StayTimer -= Time.deltaTime;
        }
        if (StayTimer < 0) 
        {
            shouldRotate = false;
            StayTimer = StayTime;
        }
    }
}
