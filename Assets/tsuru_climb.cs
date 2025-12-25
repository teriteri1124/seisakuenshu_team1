using UnityEngine;

public class tsuru_climb : MonoBehaviour
{
    public float liftSpeed = 3f;

    bool isPlayerInside;
    Rigidbody playerRb;

    void Update()
    {
        if (isPlayerInside && Input.GetKey(KeyCode.Space))
        {
            Vector3 move =
                Vector3.up * liftSpeed * Time.deltaTime;

            playerRb.MovePosition(playerRb.position + move);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb = other.GetComponent<Rigidbody>();
            isPlayerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerRb = null;
        }
    }
}
