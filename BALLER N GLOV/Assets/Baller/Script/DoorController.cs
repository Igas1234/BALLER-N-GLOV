using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public enum DoorOpenMode
    {
        Up,
        Down,
        Left,
        Right,
        Disappear
    }

    [Header("Mode Pintu")]
    public DoorOpenMode openMode = DoorOpenMode.Up;

    [Header("Pengaturan Gerak")]
    public float extraOpenDistance = 0.5f;
    public float moveSpeed = 3f;

    [Header("Pengaturan Suara")]
    public bool playSoundOnOpen = true;
    public bool playSoundOnClose = false;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private Collider2D doorCollider;
    private SpriteRenderer spriteRenderer;
    private Coroutine moveCoroutine;

    public bool disableCollider = true;

    private bool isOpen = false;

    void Start()
    {
        doorCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        closedPosition = transform.position;
        openPosition = GetOpenPosition();
    }

    private Vector3 GetOpenPosition()
    {
        float doorWidth = GetDoorWidth();
        float doorHeight = GetDoorHeight();

        switch (openMode)
        {
            case DoorOpenMode.Up:
                return closedPosition + Vector3.up * (doorHeight + extraOpenDistance);

            case DoorOpenMode.Down:
                return closedPosition + Vector3.down * (doorHeight + extraOpenDistance);

            case DoorOpenMode.Left:
                return closedPosition + Vector3.left * (doorWidth + extraOpenDistance);

            case DoorOpenMode.Right:
                return closedPosition + Vector3.right * (doorWidth + extraOpenDistance);

            case DoorOpenMode.Disappear:
                return closedPosition;

            default:
                return closedPosition;
        }
    }

    private float GetDoorWidth()
    {
        if (doorCollider != null)
            return doorCollider.bounds.size.x;

        if (spriteRenderer != null)
            return spriteRenderer.bounds.size.x;

        return transform.localScale.x;
    }

    private float GetDoorHeight()
    {
        if (doorCollider != null)
            return doorCollider.bounds.size.y;

        if (spriteRenderer != null)
            return spriteRenderer.bounds.size.y;

        return transform.localScale.y;
    }

    public void OpenDoor()
    {
        SetOpen(true);
    }

    public void CloseDoor()
    {
        SetOpen(false);
    }

    public void SetOpen(bool open)
    {
        // Suara pintu hanya bunyi kalau status pintu berubah
        if (open != isOpen && AudioManager.Instance != null)
        {
            if (open && playSoundOnOpen)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.doorSound);
            }
            else if (!open && playSoundOnClose)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.doorSound);
            }
        }

        isOpen = open;

        if (openMode == DoorOpenMode.Disappear)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !open;

            if (doorCollider != null && !disableCollider)
                doorCollider.enabled = !open;

            return;
        }

        Vector3 targetPosition = open ? openPosition : closedPosition;

        // kalau pintu ditutup, collider langsung aktif
        if (!open && doorCollider != null)
        {
            doorCollider.enabled = true;
        }

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveDoor(targetPosition, open));
    }

    private IEnumerator MoveDoor(Vector3 targetPosition, bool opening)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetPosition;

        // collider baru mati setelah pintu benar-benar terbuka
        if (opening && doorCollider != null && disableCollider)
        {
            doorCollider.enabled = false;
        }

        // kalau pintu sudah tertutup, collider tetap aktif
        if (!opening && doorCollider != null)
        {
            doorCollider.enabled = true;
        }
    }
}