using UnityEngine;
using System.Collections;

public class ItemFocusController : MonoBehaviour
{

    public HourglassController hourglassController;
    public GameObject camera;
    public Transform target;
    public float moveSpeed = 5f; // 移动速度（推荐值3-8）
    public bool isCanMove;
    public ItemSequenceManager itemSequenceManager;
    void Update()
    {
        if (isCanMove)
        {
            camera.GetComponent<CameraController>().IsCanFollow = false;
            Vector3 targetPos = new Vector3(
                target.position.x,
                target.position.y,
                camera.transform.position.z
            );

            // 平滑移动到目标
            camera.transform.position = Vector3.Lerp(
                camera.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<HourglassController>() != null)
        {

            hourglassController.IsCanControl = false;
            isCanMove = true;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            itemSequenceManager.startFirstItemFade = true;
        }
    }

}