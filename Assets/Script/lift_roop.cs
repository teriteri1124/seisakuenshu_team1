using UnityEngine;

public class lift_roop : MonoBehaviour
{
    [Header("Injected from Spawner")]
    [HideInInspector] public lever lever;
    [HideInInspector] public float speed;
    [HideInInspector] public float destroyUpperY;
    [HideInInspector] public float destroyLowerY;

    [Header("Visual")]
    public GameObject lift_Up;
    public GameObject lift_Down;

    void Update()
    {
        if (lever == null) return;

        bool isUp = lever.isUp;

        // 移動
        Vector3 dir = isUp ? Vector3.up : Vector3.down;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        // 見た目切替
        UpdateVisual(isUp);

        // 破壊判定
        float y = transform.position.y;

        if (isUp && y >= destroyUpperY)
            Destroy(gameObject);
        else if (!isUp && y <= destroyLowerY)
            Destroy(gameObject);
    }

    void UpdateVisual(bool isUp)
    {
        if (lift_Up != null)
            lift_Up.SetActive(isUp);

        if (lift_Down != null)
            lift_Down.SetActive(!isUp);
    }
}
