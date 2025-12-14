using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BlackRenderCamera : MonoBehaviour
{
    public Shader replacementShader;

    void Start()
    {
        var cam = GetComponent<Camera>();
        if (replacementShader != null)
        {
            // 第2引数はタグ指定。空文字("")ならすべてのオブジェクトに適用。
            cam.SetReplacementShader(replacementShader, "");
        }
    }

    void OnDisable()
    {
        // 元に戻す
        GetComponent<Camera>().ResetReplacementShader();
    }
}
