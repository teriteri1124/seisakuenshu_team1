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

    [Header("Settings")]
    public float light2shadow = 500f;
    public float shadow2light = 500f;

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
            Vector3 newPos = player_light.transform.position;
            newPos.y += light2shadow;
            player_shadow.GetComponent<PlayerCtrl>().WarpTo(newPos, player_light.transform.rotation);

            newPos.y += light2shadow;
            player_screen.GetComponent<PlayerCtrl>().WarpTo(newPos, player_light.transform.rotation);
        }
        else
        {
            // 影 → 通常世界
            Vector3 newPos = player_screen.transform.position;
            newPos.y -= shadow2light;
            player_shadow.GetComponent<PlayerCtrl>().WarpTo(newPos, player_screen.transform.rotation);

            newPos.y -= shadow2light;
            player_light.GetComponent<PlayerCtrl>().WarpTo(newPos, player_screen.transform.rotation);
        }

        mainCamera.SetActive(!inShadowWorld);
        subCamera.SetActive(inShadowWorld);

        Debug.Log($"World switched → {(inShadowWorld ? "Shadow" : "Normal")}");
    }
}
