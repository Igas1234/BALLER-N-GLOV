using UnityEngine;
using System.Collections;

public class LeverSwitch : MonoBehaviour
{
    [Header("Pintu yang Dibuka")]
    public DoorController[] targetDoors;

    [Header("Jalan Rahasia yang Dimunculkan")]
    public GameObject jalanRahasia;

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

            if (targetDoors.Length > 0 && targetDoors[0] != null)
            {
                scriptKamera.target = targetDoors[0].transform;
                yield return new WaitForSeconds(1f);
            }
        }

        OpenAllDoors();
        if (jalanRahasia != null)
            jalanRahasia.SetActive(true);

        Debug.Log("Pintu sedang terbuka, kamera menyorot!");

        yield return new WaitForSeconds(waktuTungguKamera);

        if (scriptKamera != null && targetAsli != null)
            scriptKamera.target = targetAsli;
    }

    private void OpenAllDoors()
    {
        foreach (DoorController door in targetDoors)
        {
            if (door != null)
                door.OpenDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Serangan") && !isActivated)
        {
            isActivated = true;
            animator.SetBool("On", true);
            StartCoroutine(BukaPintuDenganCutscene());
        }
    }
}