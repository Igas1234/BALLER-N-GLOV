using UnityEngine;

public class ShurikenTrap : MonoBehaviour
{
    [Header("Gerakan Kiri Kanan")]
    public float moveSpeed = 3f;
    public float moveDistance = 4f;
    private Vector2 startPosition;
    private bool movingRight = true;

    [Header("Rotasi")]
    public float rotateSpeed = 360f;

    [Header("Suara Shuriken")]
    public AudioClip shurikenSound;
    public float volume = 0.25f;

    private AudioSource audioSource;

    void Start()
    {
        startPosition = transform.position;

        SetupAudio();
    }

    void Update()
    {
        RotateShuriken();
        MoveLeftRight();
    }

    void RotateShuriken()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    void MoveLeftRight()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime, Space.World);

            if (transform.position.x >= startPosition.x + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);

            if (transform.position.x <= startPosition.x - moveDistance)
            {
                movingRight = true;
            }
        }
    }

    private void SetupAudio()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Kalau belum diisi manual, ambil dari AudioManager
        if (shurikenSound == null && AudioManager.Instance != null)
        {
            shurikenSound = AudioManager.Instance.shurikenSound;
        }

        audioSource.clip = shurikenSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        // 0 = suara 2D, tidak hilang walau player jauh
        audioSource.spatialBlend = 0f;

        if (shurikenSound != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Shuriken sound belum diisi di Inspector atau AudioManager.");
        }
    }
}