using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Pengaturan Kamera")]
    public Transform target;       // Objek yang akan diikuti (Player)
    public float smoothSpeed = 5f; // Kecepatan kamera menyusul player
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Posisi Z kamera harus -10

    // Kita menggunakan LateUpdate untuk kamera agar tidak patah-patah (jitter)
    void LateUpdate()
    {
        // Pastikan target (Player) tidak kosong
        if (target != null)
        {
            // Menentukan posisi yang seharusnya dituju kamera
            Vector3 targetPosition = target.position + offset;

            // Menggerakkan kamera secara halus (Lerp) menuju posisi target
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}