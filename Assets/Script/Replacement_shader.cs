using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShadowCameraRenderer2 : MonoBehaviour
{
    public Shader replacementShader; // Custom/ShadowReplace を指定

    void Start()
    {
        var cam = GetComponent<Camera>();

        // replacementShader が Inspector で指定されていればそれを使い、
        // なければ Shader.Find() で探す
        if (replacementShader == null)
        {
            replacementShader = Shader.Find("Custom/ShadowReplace");
        }

        if (replacementShader != null)
        {
            cam.SetReplacementShader(replacementShader, "");
        }
        else
        {
            Debug.LogWarning("❗ Replacement shader not found. Make sure 'Custom/ShadowReplace' exists.");
        }
    }

    void OnDisable()
    {
        // カメラが無効になったら元に戻す
        GetComponent<Camera>().ResetReplacementShader();
    }
}
