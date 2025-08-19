using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerBack : MonoBehaviour
{
    public GameObject camera;
    public Transform target;
    public GameObject Player;
    public float moveSpeed = 5f;
    public bool Isback; // 移动速度（推荐值3-8）
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Isback == true)
        {
            camera.GetComponent<CameraController>().IsCanFollow = true;
            Player.GetComponent<HourglassController>().IsCanControl = true;
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
    public void back()
    {
        Isback = true;
    }
}
