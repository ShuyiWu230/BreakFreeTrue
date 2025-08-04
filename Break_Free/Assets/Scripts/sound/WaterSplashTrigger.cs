using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplashTrigger : MonoBehaviour
{
    public AudioClip splashSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 确保沙漏有这个 tag！
        {
            Debug.Log("进入水体触发区！");
            AudioSource.PlayClipAtPoint(splashSound, transform.position, 1f);
        }
    }
}

