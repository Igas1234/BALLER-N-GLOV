using UnityEngine;
using System.Collections;

public class LeverSwitch : MonoBehaviour
{
    [Header("Pintu yang Dibuka")]
    public DoorController[] targetDoors;

    [Header("Jalan Rahasia yang Dimunculkan")]
    public SecretPathController jalanRahasia;

    [Header("Pengaturan Kamera (Cutscene)")]
    public CameraFollow scriptKamera;
    public float waktuTungguKamera = 1.5f;

    private bool isActivated = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private IEnumerator BukaPintuDenganCutscene()
    {
        Transform targetAsli = null;

        if (scriptKamera != null)
        {
            targetAsli = scriptKamera.target;

            // Kamera fokus ke pintu pertama kalau ada pintu
            if (targetDoors.Length > 0 && targetDoors[0] != null)
            {
                scriptKamera.target = targetDoors[0].transform;
                yield return new WaitForSeconds(1f);
            }
            // Kalau tidak ada pintu, kamera fokus ke jalan rahasia
            else if (jalanRahasia != null)
            {
                scriptKamera.target = jalanRahasia.transform;
                yield return new WaitForSeconds(1f);
            }
        }

        OpenAllDoors();

        if (jalanRahasia != null)
        {
            jalanRahasia.OpenPath();
        }

        Debug.Log("Lever aktif, pintu/jalan rahasia terbuka!");

        yield return new WaitForSeconds(waktuTungguKamera);

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
        if (collision.CompareTag("Serangan") && !isActivated)
        {
            isActivated = true;

            // Suara switch / lever
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.switchSound);
            }

            if (animator != null)
            {
                animator.SetBool("On", true);
            }

            StartCoroutine(BukaPintuDenganCutscene());
        }
    }
}