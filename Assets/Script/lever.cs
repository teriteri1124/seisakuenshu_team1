using UnityEngine;   // ← これが必要！！

public class lever : MonoBehaviour
{
    public bool isUp = true;
    public bool hasSwitched = false;

    public KeyCode interactKey = KeyCode.E;

    private bool canInteract = false;

    void Update()
    {
        if (canInteract && Input.GetKeyDown(interactKey))
        {
            ToggleLever();
        }
    }

    void ToggleLever()
    {
        isUp = !isUp;
        hasSwitched = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}
