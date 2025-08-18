using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerCon : MonoBehaviour
{
    //以speed的速度向前移动
    public float speed;
    Collider2D thisCollider;

    void Start()
    {
        thisCollider = this.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(0, 0, speed*Time.deltaTime);

        if (this.transform.position.z < -13) 
        {
            Destroy(this.gameObject);
        }

        if (this.transform.position.z < -10 && this.transform.position.z > -12)
        {
            thisCollider.enabled = true;
        }
        else thisCollider.enabled = false;

       
    }
}
