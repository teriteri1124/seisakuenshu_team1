using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShadowCameraRenderer : MonoBehaviour
{
    public Shader replacementShader; // Custom/ShadowReplace を指定

    void Start()
    {
        var cam = GetComponent<Camera>();
        if (replacementShader != null)
        {
            // 通常のレンダリングを置き換える
            cam.SetReplacementShader(replacementShader, "");
        }
    }
}
