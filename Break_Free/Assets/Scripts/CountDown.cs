using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Text CountDownText;
    public float CountTime;
    private float Timer;
    // Start is called before the first frame update
    void Start()
    {
        Timer = CountTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (Timer >= 10) 
        {
            Timer -= Time.deltaTime;
            CountDownText.text = "0:" + Timer.ToString("F0");
        }
        if (Timer >= 0 && Timer < 10) 
        {

            Timer -= Time.deltaTime;
            CountDownText.text = "0:0" + Timer.ToString("F0");
        }

        if (Timer == 0) 
        {
            CountDownText.text = "0:00";
        }
    }
      
    
}
