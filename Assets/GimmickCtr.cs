using UnityEngine;

public class GimmickCtr : MonoBehaviour
{
    [Header("Lever")]
    public lever leverScript;

    [Header("Destroy Targets (順番に壊す)")]
    public GameObject destroyTargetA;
    public GameObject destroyTargetB;

    [Header("Move Object (Y axis)")]
    public Transform moveTarget;
    public float moveSpeed = 2f;
    public float targetY = 5f;

    private int destroyCount = 0;
    private bool isMoving = false;
    private bool lastLeverState;

    void Start()
    {
        lastLeverState = leverScript.isUp;
    }

    void Update()
    {
        // レバーの切り替えを検知
        if (leverScript.isUp != lastLeverState)
        {
            OnLeverSwitched();
            lastLeverState = leverScript.isUp;
        }

        // 移動処理
        if (isMoving)
        {
            MoveObject();
        }
    }

    void OnLeverSwitched()
    {
        // 破壊は最大2回まで
        if (destroyCount == 0 && destroyTargetA != null)
        {
            Destroy(destroyTargetA);
            destroyCount++;
            return;
        }

        if (destroyCount == 1 && destroyTargetB != null)
        {
            Destroy(destroyTargetB);
            destroyCount++;

            // 2個目が壊れたら移動開始
            isMoving = true;
        }
    }

    void MoveObject()
    {
        if (moveTarget == null) return;

        Vector3 pos = moveTarget.position;
        pos.y += moveSpeed * Time.deltaTime;

        // 目標Yに到達したら停止
        if (pos.y >= targetY)
        {
            pos.y = targetY;
            isMoving = false;
        }

        moveTarget.position = pos;
    }
}
