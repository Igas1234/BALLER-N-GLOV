using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public DoorController targetDoor;
    private int insideCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Kotak"))
        {
            insideCount++;
            if (targetDoor != null)
                targetDoor.SetOpen(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Kotak"))
        {
            insideCount--;

            if (insideCount <= 0)
            {
                insideCount = 0;
                if (targetDoor != null)
                    targetDoor.SetOpen(false);
            }
        }
    }
}