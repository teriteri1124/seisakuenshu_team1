using UnityEngine;

public class camera_follow : MonoBehaviour
{
    public Transform player;      // プレイヤーのTransform
    public Vector3 offset;        // カメラのオフセット（位置のずれ）
    public float smoothSpeed = 5f; // 追従のスムーズさ（大きいほど速く追う）

    void LateUpdate()
    {
        if (player == null) return;

        // 目標位置を計算
        Vector3 targetPosition = player.position + offset;

        // スムーズにカメラを移動
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
