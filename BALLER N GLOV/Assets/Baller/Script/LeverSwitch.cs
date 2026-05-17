using UnityEngine;

public class LeverSwitch : MonoBehaviour
{
    [Header("Pintu yang Dibuka")]
    public DoorController[] targetDoors;

    private bool playerNearby = false;
    private bool isActivated = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            isActivated = true;

            OpenAllDoors();

            Debug.Log("Tuas ditekan dengan E, semua pintu terbuka!");
        }
    }

    private void OpenAllDoors()
    {
        foreach (DoorController door in targetDoors)
        {
            if (door != null)
            {
                door.OpenDoor();
            }
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