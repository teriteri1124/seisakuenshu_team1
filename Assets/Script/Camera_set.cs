using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject subCamera;

    [Header("Players")]
    public GameObject player_light;
    public GameObject player_shadow;
    public GameObject player_screen;

    [Header("Y Offset Settings")]
    public float light2shadow = 500f;
    public float shadow2light = 500f;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float firstRayHeight = 0.5f;
    [SerializeField] private float fallbackRayHeight = 5f;
    [SerializeField] private float groundCheckDistance = 20f;
    [SerializeField] private float groundOffset = 0.1f;

    private bool inShadowWorld = false;

    void Start()
    {
        if (mainCamera == null) mainCamera = GameObject.Find("Main Camera");
        if (subCamera == null) subCamera = GameObject.Find("Sub Camera");

        subCamera.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleWorld();
        }
    }

    void ToggleWorld()
    {
        inShadowWorld = !inShadowWorld;

        if (inShadowWorld)
        {
            // 通常 → 影の世界
            Vector3 basePos = player_light.transform.position;
            Quaternion rot = player_light.transform.rotation;

            Vector3 shadowPos = basePos;
            shadowPos.y += light2shadow;
            shadowPos = GetSafePosition(shadowPos);
            player_shadow.GetComponent<PlayerCtrl>().WarpTo(shadowPos, rot);

            Vector3 screenPos = basePos;
            screenPos.y += light2shadow * 2f;
            screenPos = GetSafePosition(screenPos);
            player_screen.GetComponent<PlayerCtrl>().WarpTo(screenPos, rot);
        }
        else
        {
            // 影 → 通常世界
            Vector3 basePos = player_screen.transform.position;
            Quaternion rot = player_screen.transform.rotation;

            Vector3 shadowPos = basePos;
            shadowPos.y -= shadow2light;
            shadowPos = GetSafePosition(shadowPos);
            player_shadow.GetComponent<PlayerCtrl>().WarpTo(shadowPos, rot);

            Vector3 lightPos = basePos;
            lightPos.y -= shadow2light * 2f;
            lightPos = GetSafePosition(lightPos);
            player_light.GetComponent<PlayerCtrl>().WarpTo(lightPos, rot);
        }

        mainCamera.SetActive(!inShadowWorld);
        subCamera.SetActive(inShadowWorld);

        Debug.Log($"World switched → {(inShadowWorld ? "Shadow" : "Normal")}");
    }

    Vector3 GetSafePosition(Vector3 targetPos)
    {
        RaycastHit hit;

        // ① 想定位置から下方向に探索
        Vector3 origin1 = targetPos + Vector3.up * firstRayHeight;
        if (Physics.Raycast(
            origin1,
            Vector3.down,
            out hit,
            groundCheckDistance,
            groundLayers,
            QueryTriggerInteraction.Ignore))
        {
            targetPos.y = hit.point.y + groundOffset;
            return targetPos;
        }

        // ② 見つからなければ、より高い位置から探索
        Vector3 origin2 = targetPos + Vector3.up * fallbackRayHeight;
        if (Physics.Raycast(
            origin2,
            Vector3.down,
            out hit,
            groundCheckDistance + fallbackRayHeight,
            groundLayers,
            QueryTriggerInteraction.Ignore))
        {
            targetPos.y = hit.point.y + groundOffset;
            return targetPos;
        }

        // ③ 最後の保険（補正なし）
        Debug.LogWarning("床が見つかりませんでした。想定位置を使用します。");
        return targetPos;
    }
}
