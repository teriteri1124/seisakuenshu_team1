using UnityEngine;

public class lift : MonoBehaviour
{
    [Header("Reference")]
    public lever lever;

    [Header("Move Settings")]
    public float speed = 2f;
    public float upperY = 5f;   // 上限
    public float lowerY = 0f;   // 下限
    public bool moveOnlyWhenLeverOn = true;

    [Header("Visual Objects")]
    public GameObject liftUpVisual;
    public GameObject liftDownVisual;

    private bool isMoving = false;

    void Start()
    {
        UpdateVisual();
    }

    void Update()
    {
        if (lever == null) return;

        // レバーを操作するまで動かさない
        if (moveOnlyWhenLeverOn && !lever.hasSwitched)
        {
            return;
        }

        float currentY = transform.position.y;

        if (lever.isUp)
        {
            if (currentY >= upperY)
            {
                StopMove();
                return;
            }

            Move(Vector3.up);
        }
        else
        {
            if (currentY <= lowerY)
            {
                StopMove();
                return;
            }

            Move(Vector3.down);
        }

        UpdateVisual();
    }

    void Move(Vector3 dir)
    {
        isMoving = true;
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void StopMove()
    {
        isMoving = false;
    }

    void UpdateVisual()
    {
        liftUpVisual.SetActive(lever.isUp);
        liftDownVisual.SetActive(!lever.isUp);
    }
}
