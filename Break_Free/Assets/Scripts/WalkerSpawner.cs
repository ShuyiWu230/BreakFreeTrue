using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerSpawner : MonoBehaviour
{
    //本脚本用于控制行人的生成
    //生成逻辑为，三个点不同时生成同一类型的人，保证关卡可以进行
    public GameObject jellyfishP, rabbitP, snailP,catP;//生成的四种prefab
    public float initialDelay;//初始的等待时间
    public float spawnInterval;//生成的间隔时间
    public Transform spawnPoint;//生成位置，默认填自己的位置
    public GameObject prefabForSpawn;//下一个生成的预制体

    public int spawnNum;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine() 
    {
        yield return new WaitForSeconds(initialDelay);//初始等待时间

        while (true) 
        {
            SpawnPrefab();

            yield return new WaitForSeconds(spawnInterval);//间隔等待时间
        }
    }

    void SpawnPrefab() 
    {
        
        spawnNum = Random.Range(1, 5);//1，2，3中抽一个数
        if (spawnNum == 1) prefabForSpawn = jellyfishP;
        if (spawnNum == 2) prefabForSpawn = rabbitP;
        if (spawnNum == 3) prefabForSpawn = snailP;
        if (spawnNum == 4) prefabForSpawn = catP;
        Instantiate(prefabForSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}
