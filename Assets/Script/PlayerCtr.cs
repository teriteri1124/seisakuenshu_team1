using UnityEngine;

public class PlayerCtr : MonoBehaviour
{
    private Vector3 movingDirection;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;  // ジャンプ力
    private Rigidbody rb;
    private Vector3 movingVelocity;

    private bool isGrounded = true;  // 地面にいるかどうか判定用

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        movingDirection = new Vector3(x, 0, z);
        movingDirection.Normalize();

        movingVelocity = movingDirection * speed;

        // ジャンプ判定：スペースキーが押されていて、かつ地面にいる時
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Y方向の速度はそのままにして横移動だけ反映
        rb.linearVelocity = new Vector3(movingVelocity.x, rb.linearVelocity.y, movingVelocity.z);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    // 地面に接触したら呼ばれる関数
    private void OnCollisionEnter(Collision collision)
    {
        // もし接触したオブジェクトが地面タグなら
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
