using System.Collections;
using UnityEngine;

public class EnemyLebah : MonoBehaviour
{
    public enum PatrolMode
    {
        Horizontal,
        Vertical
    }

    [Header("Pengaturan Patroli")]
    public float speed = 3f;
    public float patrolDistance = 3f;
    public PatrolMode patrolMode = PatrolMode.Horizontal;

    [Header("Arah Sprite")]
    public bool spriteMenghadapKananSaatScalePositif = true;

    [Header("Pengaturan Suara Tawon")]
    public AudioClip tawonSound;
    public float tawonVolume = 0.2f;
    public float jarakMulaiSuara = 5f;
    public float jarakBerhentiSuara = 6f;

    private AudioSource audioSource;
    private Transform playerTarget;

    private Vector2 startPosition;
    private bool movingPositive = true;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponentInChildren<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }

        SetupAudio();
    }

    void Update()
    {
        if (isDead) return;

        if (patrolMode == PatrolMode.Horizontal)
        {
            PatrolHorizontal();
            FlipHorizontal();
        }
        else if (patrolMode == PatrolMode.Vertical)
        {
            PatrolVertical();
        }

        HandleTawonSound();
    }

    private void SetupAudio()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Kalau belum diisi manual, ambil dari AudioManager
        if (tawonSound == null && AudioManager.Instance != null)
        {
            tawonSound = AudioManager.Instance.tawonSound;
        }

        audioSource.clip = tawonSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = tawonVolume;

        // Suara dibuat 2D, jarak kita atur sendiri lewat script
        audioSource.spatialBlend = 0f;
    }

    private void HandleTawonSound()
    {
        if (playerTarget == null || audioSource == null || tawonSound == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        // Kalau player mendekat, suara mulai
        if (distanceToPlayer <= jarakMulaiSuara)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        // Kalau player menjauh, suara berhenti
        if (distanceToPlayer >= jarakBerhentiSuara)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void PatrolHorizontal()
    {
        if (movingPositive)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (transform.position.x >= startPosition.x + patrolDistance)
            {
                movingPositive = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (transform.position.x <= startPosition.x - patrolDistance)
            {
                movingPositive = true;
            }
        }
    }

    private void PatrolVertical()
    {
        if (movingPositive)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);

            if (transform.position.y >= startPosition.y + patrolDistance)
            {
                movingPositive = false;
            }
        }
        else
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);

            if (transform.position.y <= startPosition.y - patrolDistance)
            {
                movingPositive = true;
            }
        }
    }

    private void FlipHorizontal()
    {
        Vector3 scale = transform.localScale;

        if (spriteMenghadapKananSaatScalePositif)
        {
            scale.x = movingPositive ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = movingPositive ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Serangan") && !isDead)
        {
            isDead = true;

            // Hentikan suara tawon saat mati
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Suara enemy mati
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDeath);
            }

            if (animator != null)
            {
                animator.SetTrigger("mati");
            }

            StartCoroutine(MatiRoutine());
        }

        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.TakeDamage(1);
            }
        }
    }

    private IEnumerator MatiRoutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}