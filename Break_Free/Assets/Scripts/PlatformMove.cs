using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    //本脚本用于控制平台的移动
    // Start is called before the first frame update
    public float speed;
    public Transform targetPos;
    Vector3 startPos;
    void Start()
    {
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos.position, Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, targetPos.position) < 0.01f) 
        {
            this.transform.position = startPos;
        }
    }
}
