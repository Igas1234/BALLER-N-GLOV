using UnityEngine;

public class LeverSwitch : MonoBehaviour
{
    public DoorController targetDoor;

    private bool playerNearby = false;
    private bool isActivated = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            isActivated = true;

            if (targetDoor != null)
            {
                targetDoor.OpenDoor();
            }

            Debug.Log("Tuas ditekan dengan E, pintu terbuka!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Player dekat tuas. Tekan E untuk membuka pintu.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}