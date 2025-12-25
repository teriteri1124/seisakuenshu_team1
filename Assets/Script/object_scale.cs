using UnityEngine;

public class SmoothScaleByCameraDistance3D : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Scale Targets")]
    public Transform scaleTargetA;
    public Transform scaleTargetB;

    [Header("Distance Settings")]
    public float nearDistance = 2f;     // 近いほど大きい
    public float farDistance = 10f;     // 遠いほど小さい

    [Header("Scale Settings")]
    public Vector3 minScale = Vector3.one;               // 遠いとき
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);   // 近いとき

    [Header("Smooth Settings")]
    public float smoothSpeed = 5f;      // なめらかさ（大きいほど速い）

    void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // 3D距離を取得（移動処理はしない）
        float distance = Vector3.Distance(
            transform.position,
            cameraTransform.position
        );

        // 近いほど 1、遠いほど 0
        float t = Mathf.InverseLerp(farDistance, nearDistance, distance);

        // 距離に応じた「目標スケール」
        Vector3 targetScale = Vector3.Lerp(minScale, maxScale, t);

        // なめらかに追従させる
        scaleTargetA.localScale = Vector3.Lerp(
            scaleTargetA.localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );

        scaleTargetB.localScale = Vector3.Lerp(
            scaleTargetB.localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );
    }
}
