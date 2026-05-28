using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Pintu")]
    public DoorController targetDoor;

    [Header("Pengaturan Gerak Plate")]
    public float pressedOffsetY = -0.2f;
    public float moveSpeed = 5f;

    private int insideCount = 0;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.localPosition;
        targetPosition = startPosition;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger masuk: " + other.gameObject.name + " tag: " + other.tag);
        if (other.CompareTag("Player") || other.CompareTag("Kotak"))
        {
            insideCount++;
            targetPosition = new Vector3(startPosition.x, startPosition.y + pressedOffsetY, startPosition.z);
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
                targetPosition = startPosition;
                if (targetDoor != null)
                    targetDoor.SetOpen(false);
            }
        }
    }
}