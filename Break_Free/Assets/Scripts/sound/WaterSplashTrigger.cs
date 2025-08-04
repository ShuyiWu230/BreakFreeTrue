using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplashTrigger : MonoBehaviour
{
    public AudioClip splashSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ȷ��ɳ©����� tag��
        {
            Debug.Log("����ˮ�崥������");
            AudioSource.PlayClipAtPoint(splashSound, transform.position, 1f);
        }
    }
}

