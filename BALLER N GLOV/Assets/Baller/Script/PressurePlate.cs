using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [Header("Pintu")]
    public DoorController targetDoor;

    [Header("Pengaturan Gerak Plate")]
    public float pressedOffsetY = -0.2f;
    public float moveSpeed = 5f;

    [Header("Pengaturan Kamera")]
    public CameraFollow scriptKamera;
    public Transform targetFokusPintu;
    public float waktuKameraKePintu = 1f;
    public float waktuKameraMenyorot = 1.5f;

    private int insideCount = 0;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private bool sedangFokusKamera = false;
    private Transform targetAsliKamera;

    void Start()
    {
        startPosition = transform.localPosition;
        targetPosition = startPosition;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition,
            Time.deltaTime * moveSpeed
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // HANYA KOTAK yang bisa menekan pressure plate
        if (other.CompareTag("Kotak"))
        {
            insideCount++;

            targetPosition = new Vector3(
                startPosition.x,
                startPosition.y + pressedOffsetY,
                startPosition.z
            );

            // Suara pressure plate hanya saat kotak pertama kali menekan
            if (insideCount == 1 && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.pressurePlateSound);
            }

            if (targetDoor != null)
            {
                targetDoor.SetOpen(true);
            }

            // Kamera fokus ke pintu hanya saat kotak pertama kali masuk
            if (insideCount == 1 && !sedangFokusKamera)
            {
                StartCoroutine(FokusKameraKePintu());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // HANYA KOTAK yang dihitung keluar
        if (other.CompareTag("Kotak"))
        {
            insideCount--;

            if (insideCount <= 0)
            {
                insideCount = 0;
                targetPosition = startPosition;

                if (targetDoor != null)
                {
                    targetDoor.SetOpen(false);
                }
            }
        }
    }

    private IEnumerator FokusKameraKePintu()
    {
        sedangFokusKamera = true;

        if (scriptKamera != null)
        {
            // Simpan target asli kamera, biasanya Player
            targetAsliKamera = scriptKamera.target;

            // Kalau Target Fokus Pintu diisi, kamera fokus ke situ
            if (targetFokusPintu != null)
            {
                scriptKamera.target = targetFokusPintu;
            }
            // Kalau kosong, kamera fokus langsung ke pintu
            else if (targetDoor != null)
            {
                scriptKamera.target = targetDoor.transform;
            }

            // Tunggu kamera bergerak menuju pintu
            yield return new WaitForSeconds(waktuKameraKePintu);

            // Tahan kamera sebentar di pintu
            yield return new WaitForSeconds(waktuKameraMenyorot);

            // Balik lagi ke player
            if (targetAsliKamera != null)
            {
                scriptKamera.target = targetAsliKamera;
            }
        }

        sedangFokusKamera = false;
    }
}