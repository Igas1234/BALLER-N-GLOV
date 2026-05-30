using UnityEngine;
using System.Collections;

public class SecretPathController : MonoBehaviour
{
    public enum SecretPathMode
    {
        Appear,
        RiseUp
    }

    [Header("Mode Jalan Rahasia")]
    public SecretPathMode mode = SecretPathMode.RiseUp;

    [Header("Pengaturan Gerak")]
    public float hiddenOffsetY = -2f;
    public float moveSpeed = 3f;

    [Header("Pengaturan Awal")]
    public bool hiddenOnStart = true;

    [Header("Pengaturan Suara")]
    public bool playSoundOnOpen = true;
    public bool playSoundOnClose = false;

    private Vector3 shownPosition;
    private Vector3 hiddenPosition;

    private Renderer pathRenderer;
    private Collider2D[] pathColliders;
    private Coroutine moveCoroutine;

    private bool isOpen = false;

    void Start()
    {
        pathRenderer = GetComponent<Renderer>();
        pathColliders = GetComponents<Collider2D>();

        // Posisi di Scene dianggap sebagai posisi saat jalan sudah muncul
        shownPosition = transform.position;

        // Posisi tersembunyi ada di bawah
        hiddenPosition = shownPosition + new Vector3(0f, hiddenOffsetY, 0f);

        if (hiddenOnStart)
        {
            if (mode == SecretPathMode.RiseUp)
            {
                // Jalan rahasia dipindahkan ke bawah, tapi visualnya tetap hidup
                transform.position = hiddenPosition;
                SetVisible(true);
                SetCollider(false);
            }
            else if (mode == SecretPathMode.Appear)
            {
                // Kalau mode Appear, baru benar-benar disembunyikan
                SetVisible(false);
                SetCollider(false);
            }

            isOpen = false;
        }
        else
        {
            isOpen = true;
        }
    }

    public void OpenPath()
    {
        SetOpen(true);
    }

    public void ClosePath()
    {
        SetOpen(false);
    }

    public void SetOpen(bool open)
    {
        // Suara jalan rahasia / pintu hanya bunyi kalau status berubah
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

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        if (mode == SecretPathMode.Appear)
        {
            SetVisible(open);
            SetCollider(open);
            return;
        }

        if (mode == SecretPathMode.RiseUp)
        {
            if (open)
            {
                // Saat dibuka, jalan terlihat dan naik ke posisi asli
                SetVisible(true);
                SetCollider(false);
                moveCoroutine = StartCoroutine(MovePath(shownPosition, true));
            }
            else
            {
                // Saat ditutup, jalan turun lagi ke bawah
                SetCollider(false);
                moveCoroutine = StartCoroutine(MovePath(hiddenPosition, false));
            }
        }
    }

    private IEnumerator MovePath(Vector3 targetPosition, bool opening)
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

        if (opening)
        {
            // Collider aktif setelah jalan sudah sampai atas
            SetCollider(true);
        }
        else
        {
            // Kalau sudah turun, collider tetap mati
            SetCollider(false);

            // Untuk mode RiseUp, visual jangan dimatikan.
            // Biar dia tetap "ada di bawah".
            SetVisible(true);
        }
    }

    private void SetVisible(bool visible)
    {
        if (pathRenderer != null)
        {
            pathRenderer.enabled = visible;
        }
    }

    private void SetCollider(bool active)
    {
        foreach (Collider2D col in pathColliders)
        {
            if (col != null)
            {
                col.enabled = active;
            }
        }
    }
}