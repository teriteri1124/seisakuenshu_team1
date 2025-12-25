using UnityEngine;
using System.Collections;

public class lift_spawner : MonoBehaviour
{
    [Header("Reference")]
    public lift_roop liftPrefab;
    public lever lever;

    [Header("Lever Option")]
    public bool useLever = true; // ★ レバーを使うか
    public bool fixedUp = true;  // ★ レバー無効時の向き

    [Header("Move Settings")]
    public float speed = 2f;

    [Header("Y Settings")]
    public float spawnY_Up = 0f;
    public float destroyY_Up = 10f;

    public float spawnY_Down = 10f;
    public float destroyY_Down = 0f;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;

    bool lastLeverState;

    void Start()
    {
        if (useLever && lever != null)
            lastLeverState = lever.isUp;

        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        if (!useLever || lever == null) return;

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
        // 今回は特に処理なし（将来拡張用）
    }

    void Spawn()
    {
        // ★ 向き判定を一本化
        bool isUp =
            useLever && lever != null
            ? lever.isUp
            : fixedUp;

        float spawnY = isUp ? spawnY_Up : spawnY_Down;
        float destroyUpper = isUp ? destroyY_Up : destroyY_Down;
        float destroyLower = isUp ? destroyY_Down : destroyY_Up;

        Vector3 spawnPos = transform.position;
        spawnPos.y = spawnY;

        lift_roop lift =
            Instantiate(liftPrefab, spawnPos, Quaternion.identity);

        // 注入
        lift.lever = useLever ? lever : null;
        lift.speed = speed;
        lift.destroyUpperY = destroyUpper;
        lift.destroyLowerY = destroyLower;
    }
}
