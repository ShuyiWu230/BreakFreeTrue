using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    [SerializeField] private float bounceForce = 20f; // 弹跳力度

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<HourglassController>() != null)
        {

            Debug.Log("1");
            // 重置垂直速度并施加弹跳力
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(other.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        }
    }
}
