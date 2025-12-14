using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchToChangeScene : MonoBehaviour
{
    // 触れた相手がこのタグだったら遷移する
    public string targetTag = "Player";

    // 遷移するシーン名
    public string nextSceneName = "cave scene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
