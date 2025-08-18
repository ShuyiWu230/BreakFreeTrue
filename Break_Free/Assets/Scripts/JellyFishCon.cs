using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JellyFishCon : MonoBehaviour
{
    //在消失的时候，留下一坨水
    public GameObject JellyWater;
    public GameObject Water1, Water2, Water3;
    //左边：-11.5
    //中间：-6.61
    //右边：-2.2
    // Start is called before the first frame update
    void Start()
    {
        Water1 = GameObject.Find("JellyWater (1)");
        Water2 = GameObject.Find("JellyWater (2)");
        Water3 = GameObject.Find("JellyWater (3)");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x <= -10) JellyWater = Water1;
        if (this.transform.position.x > -10 && this.transform.position.x < -3) JellyWater = Water2;
        if (this.transform.position.x >= -3) JellyWater = Water3;
        if (this.transform.position.z < -12)
        {

            JellyWater.GetComponent<JellyWaterCon>().WaterOn = true;
        }


    }
}
