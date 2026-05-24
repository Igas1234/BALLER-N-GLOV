using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLebah : MonoBehaviour
{
    [Header("Pengaturan Patroli")]
    public float speed = 3f;           // Kecepatan terbang
    public float patrolDistance = 3f;  // Jarak tempuh sebelum putar balik

    private Vector2 startPosition;     // Mengingat posisi awal
    private bool movingRight = true;   // Penanda arah gerak

    void Start()
    {
        // Menyimpan posisi awal saat game pertama kali dijalankan
        startPosition = transform.position;
    }

    void Update()
    {
        if (movingRight)
        {
            // Terbang ke Kanan
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // Jika sudah terlalu jauh ke kanan, putar balik ke kiri
            if (transform.position.x >= startPosition.x + patrolDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            // Terbang ke Kiri
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // Jika sudah terlalu jauh ke kiri, putar balik ke kanan
            if (transform.position.x <= startPosition.x - patrolDistance)
            {
                movingRight = true;
            }
        }
        Vector3 scale = transform.localScale;
        scale.x = movingRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
    // OnTriggerEnter2D digunakan untuk mendeteksi Collider yang statusnya "Is Trigger"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kalau yang masuk ke badannya punya tag "Serangan", lebahnya mati
        if (collision.gameObject.CompareTag("Serangan"))
        {
            Destroy(gameObject); // Menghancurkan objek lebah ini dari dalam game
        }
    }
}