using UnityEngine;
using System.Collections;

public class LeverSwitch : MonoBehaviour
{
    [Header("Pintu yang Dibuka")]
    public DoorController[] targetDoors;

    [Header("Jalan Rahasia yang Dimunculkan")]
    public GameObject jalanRahasia;

    [Header("Pengaturan Kamera (Cutscene)")]
    public CameraFollow scriptKamera;      // Kolom untuk memasukkan Main Camera
    public float waktuTungguKamera = 1.5f; // Lama kamera menyorot pintu terbuka

    private bool playerNearby = false;
    private bool isActivated = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            isActivated = true;
            
            // Kita jalankan fungsi cutscene kamera dan pintu di sini
            StartCoroutine(BukaPintuDenganCutscene());
        }
    }

    // Fungsi Coroutine untuk mengatur urutan waktu cutscene
    private IEnumerator BukaPintuDenganCutscene()
    {
        Transform targetAsli = null;

        // 1. Jika kameranya sudah dimasukkan, simpan target aslinya (yaitu Player)
        if (scriptKamera != null)
        {
            targetAsli = scriptKamera.target;

            // 2. Pindahkan fokus kamera ke pintu pertama yang ada di daftar
            if (targetDoors.Length > 0 && targetDoors[0] != null)
            {
                scriptKamera.target = targetDoors[0].transform;
                
                // Tunggu 1 detik agar kamera sempat jalan ke arah pintu dulu
                yield return new WaitForSeconds(1f); 
            }
        }

        // 3. Buka semua pintu dan munculkan jalan rahasia
        OpenAllDoors();
        if (jalanRahasia != null)
        {
            jalanRahasia.SetActive(true);
        }

        Debug.Log("Pintu sedang terbuka, kamera menyorot!");

        // 4. Tunggu beberapa detik agar Player puas melihat pintunya terbuka
        yield return new WaitForSeconds(waktuTungguKamera);

        // 5. Kembalikan target kamera ke Player agar bisa lanjut main
        if (scriptKamera != null && targetAsli != null)
        {
            scriptKamera.target = targetAsli;
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