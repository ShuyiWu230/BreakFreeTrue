using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    [Header("Water Drop Settings")]
    public GameObject waterDropPrefab; // 水滴预制体
    public Transform leakPoint;        // 漏水点（水滴生成位置）
    public float dropSpawnRate = 1f;   // 每秒生成水滴数量（1=1个/秒，2=2个/秒）
    public float dropLifetime = 2f;    // 水滴存在时间（秒）
    public Vector2 dropDirection = new Vector2(0, -1); // 水滴初始方向（向下）
    public float dropSpeed = 1f;       // 水滴初始速度

    public bool isLeaking ;//是否在漏水
    private float dropSpawnTimer = 0f; // 水滴生成计时器
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 水滴生成逻辑（仅在漏水时）
        if (isLeaking && leakPoint != null && waterDropPrefab != null)
        {
            dropSpawnTimer += Time.deltaTime;
            float spawnInterval = 1f / dropSpawnRate; // 生成间隔（秒）

            if (dropSpawnTimer >= spawnInterval)
            {
                SpawnWaterDrop();
                dropSpawnTimer = 0f; // 重置计时器
            }
        }
    }


    // 生成水滴
    void SpawnWaterDrop()
    {
        // 实例化水滴
        GameObject drop = Instantiate(waterDropPrefab, leakPoint.position, Quaternion.identity);

        // 给水滴添加初速度
        Rigidbody2D rb = drop.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dropDirection * dropSpeed;
        }

        // 设置水滴自动销毁
        Destroy(drop, dropLifetime);
    }
}
