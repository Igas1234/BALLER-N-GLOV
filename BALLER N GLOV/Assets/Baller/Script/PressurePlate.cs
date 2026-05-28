using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [Header("Pintu yang Dibuka")]
    public DoorController targetDoor;

    [Header("Pengaturan Kamera")]
    public CameraFollow scriptKamera;
    public Transform targetFokusPintu;
    public float waktuKameraMenujuPintu = 1f;
    public float waktuTungguKamera = 1.5f;

    private int insideCount = 0;
    private bool sedangCutscene = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // HANYA KOTAK yang bisa mengaktifkan pressure plate
        if (other.CompareTag("Kotak"))
        {
            insideCount++;

            if (targetDoor != null)
            {
                targetDoor.SetOpen(true);
            }

            // Kamera fokus hanya saat kotak pertama kali menekan plate
            if (!sedangCutscene)
            {
                StartCoroutine(FokusKameraKePintu());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // HANYA KOTAK yang dihitung keluar dari pressure plate
        if (other.CompareTag("Kotak"))
        {
            insideCount--;

            if (insideCount <= 0)
            {
                insideCount = 0;

                if (targetDoor != null)
                {
                    targetDoor.SetOpen(false);
                }
            }
        }
    }

    private IEnumerator FokusKameraKePintu()
    {
        sedangCutscene = true;

        Transform targetAsli = null;

        if (scriptKamera != null)
        {
            // Simpan target asli kamera, biasanya Player
            targetAsli = scriptKamera.target;

            // Kamera fokus ke titik pintu kalau diisi
            if (targetFokusPintu != null)
            {
                scriptKamera.target = targetFokusPintu;
            }
            // Kalau targetFokusPintu kosong, kamera fokus langsung ke pintu
            else if (targetDoor != null)
            {
                scriptKamera.target = targetDoor.transform;
            }

            // Tunggu kamera bergerak ke pintu
            yield return new WaitForSeconds(waktuKameraMenujuPintu);

            // Tahan kamera sebentar di pintu
            yield return new WaitForSeconds(waktuTungguKamera);

            // Balik ke target asli, yaitu player
            if (targetAsli != null)
            {
                scriptKamera.target = targetAsli;
            }
        }

        sedangCutscene = false;
    }
}