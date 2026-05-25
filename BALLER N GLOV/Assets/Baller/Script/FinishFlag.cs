using UnityEngine;

public class FinishFlag : MonoBehaviour
{
    [Header("Tampilan Menang (Opsional)")]
    public GameObject canvasMenang; // Masukkan UI teks "You Win" ke sini nanti

    private bool sudahMenang = false;

    void Start()
    {
        // Pastikan UI Menang disembunyikan saat game baru mulai
        if (canvasMenang != null)
        {
            canvasMenang.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mengecek apakah yang menyentuh bendera adalah Player
        if (collision.CompareTag("Player") && !sudahMenang)
        {
            sudahMenang = true;
            GameSelesai();
        }
    }

    private void GameSelesai()
    {
        Debug.Log("Pemain menyentuh bendera! LEVEL SELESAI!");

        // 1. Munculkan Tulisan/UI Menang
        if (canvasMenang != null)
        {
            canvasMenang.SetActive(true);
        }

        // 2. Hentikan waktu agar Player dan musuh tidak bisa bergerak lagi
        Time.timeScale = 0f; 
        
        // Catatan: Jika nanti kamu ingin pindah ke level/scene lain,
        // kodenya bisa ditambahkan di sini.
    }
}