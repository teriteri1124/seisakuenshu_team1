using UnityEngine;
using System.Collections;

public class combare : MonoBehaviour
{
    [Header("Reference")]
    public lever lever;

    [Header("Move Settings")]
    public float speed = 2f;
    public float upperZ = 0f;
    public float lowerZ = 5f;
    public bool moveOnlyWhenLeverOn = true;

    [Header("Sound (AudioSource)")]
    public AudioSource switchSE; // レバー切り替え音（1回）
    public AudioSource moveSE;   // 移動中SE（ループ）

    bool isMoving = false;
    bool isWaitingForSwitchSE = false;

    void Update()
    {
        if (lever == null) return;

        if (moveOnlyWhenLeverOn && !lever.hasSwitched)
            return;

        // スイッチ音再生中 or 移動中は何もしない
        if (isWaitingForSwitchSE || isMoving)
            return;

        float currentZ = transform.position.z;

        if (lever.isUp && currentZ < upperZ)
        {
            StartCoroutine(SwitchAndMove(Vector3.forward));
        }
        else if (!lever.isUp && currentZ > lowerZ)
        {
            StartCoroutine(SwitchAndMove(Vector3.back));
        }
    }

    IEnumerator SwitchAndMove(Vector3 dir)
    {
        isWaitingForSwitchSE = true;

        // レバー切り替え音（1回）
        if (switchSE != null)
        {
            switchSE.Play();
            yield return new WaitForSeconds(switchSE.clip.length-3);
        }

        isWaitingForSwitchSE = false;
        isMoving = true;

        // 移動中SE開始
        if (moveSE != null)
        {
            moveSE.Play();
        }

        // 移動ループ
        while (isMoving)
        {
            Vector3 pos = transform.position;
            pos.z += dir.z * speed * Time.deltaTime;
            transform.position = pos;

            if (dir.z > 0 && pos.z >= upperZ)
                StopMove();
            else if (dir.z < 0 && pos.z <= lowerZ)
                StopMove();

            yield return null;
        }
    }

    void StopMove()
    {
        isMoving = false;

        if (moveSE != null && moveSE.isPlaying)
        {
            moveSE.Stop();
        }
    }
}
