using UnityEngine;
using System.Collections;

public class liftSpawnerWithoutLever : MonoBehaviour
{
    [Header("Reference")]
    public lift_roop liftPrefab;

    [Header("Direction Setting")]
    public bool isUp = true;   // true = 上昇 / false = 下降

    [Header("Move Settings")]
    public float speed = 2f;

    [Header("Y Settings")]
    public float spawnY_Up = 0f;        // 上昇時の生成Y
    public float destroyY_Up = 10f;     // 上昇時の破壊Y

    public float spawnY_Down = 10f;     // 下降時の生成Y
    public float destroyY_Down = 0f;    // 下降時の破壊Y

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void Spawn()
    {
        float spawnY = isUp ? spawnY_Up : spawnY_Down;
        float destroyUpper = isUp ? destroyY_Up : destroyY_Down;
        float destroyLower = isUp ? destroyY_Down : destroyY_Up;

        Vector3 spawnPos = transform.position;
        spawnPos.y = spawnY;

        lift_roop lift =
            Instantiate(liftPrefab, spawnPos, Quaternion.identity);

        // 注入
        lift.speed = speed;
        lift.destroyUpperY = destroyUpper;
        lift.destroyLowerY = destroyLower;
    }
}
