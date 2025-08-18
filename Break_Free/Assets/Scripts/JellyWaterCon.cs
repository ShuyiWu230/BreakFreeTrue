using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyWaterCon : MonoBehaviour
{
    public bool WaterOn;
    public float waterOnTime;//水的持续时间
    float timer;//计时器
    Color waterColor;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = waterOnTime;
        waterColor = this.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<SpriteRenderer>().color = waterColor;

        if (WaterOn == true) 
        {
            timer -= 1*Time.deltaTime;
            waterColor.a = 1;
            if (timer <= 0)
            {
                timer = waterOnTime;
                WaterOn = false;
            }

            this.GetComponentInChildren<WaterDrop>().enabled = true;

        }

        if (WaterOn == false) 
        {
            waterColor.a = 0;
            this.GetComponentInChildren<WaterDrop>().enabled = false;
        }
    }
}
