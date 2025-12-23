using UnityEngine;
using System.Collections;

public class lift_spawner : MonoBehaviour
{
    [Header("Reference")]
    public lift_roop liftPrefab;
    public lever lever;

    [Header("Move Settings")]
    public float speed = 2f;

    [Header("Y Settings")]
    public float spawnY_Up = 0f;        // 上昇時の生成Y
    public float destroyY_Up = 10f;     // 上昇時の破壊Y

    public float spawnY_Down = 10f;     // 下降時の生成Y
    public float destroyY_Down = 0f;    // 下降時の破壊Y

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;

    bool lastLeverState;

    void Start()
    {
        lastLeverState = lever.isUp;
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        // レバー切り替え検知
        if (lever.isUp != lastLeverState)
        {
            OnLeverSwitched();
            lastLeverState = lever.isUp;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void OnLeverSwitched()
    {
    }

    void Spawn()
    {
        float spawnY = lever.isUp ? spawnY_Up : spawnY_Down;
        float destroyY = lever.isUp ? destroyY_Up : destroyY_Down;

        Vector3 spawnPos = transform.position;
        spawnPos.y = spawnY;

        lift_roop lift =
            Instantiate(liftPrefab, spawnPos, Quaternion.identity);

        // 注入
        lift.lever = lever;
        lift.speed = speed;
        lift.destroyUpperY = destroyY_Up;
        lift.destroyLowerY = destroyY_Down;
    }
}
