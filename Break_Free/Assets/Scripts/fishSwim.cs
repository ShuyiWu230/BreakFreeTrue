using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishSwim : MonoBehaviour
{
    //本脚本用于控制鱼的游动
    //鱼游动一定距离后，自动传送回原点
    //鱼游动的高度为随机数
    Vector3 startPos;//起始点
    Vector3 targetPos;//目的地
    public float swimLongth;//游的长短
    float randomY;//随机高度offset
    public float speed;//游动速度
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        targetPos = this.transform.position -= new Vector3(swimLongth, 0, 0);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
       
        
        transform.position = Vector3.MoveTowards(transform.position, targetPos,  Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f) 
        {
            randomY = Random.Range(-3, 3);
            startPos += new Vector3(0, randomY, 0);
            targetPos += new Vector3(0, randomY, 0);
            this.transform.position = startPos;

        }
    }
}
