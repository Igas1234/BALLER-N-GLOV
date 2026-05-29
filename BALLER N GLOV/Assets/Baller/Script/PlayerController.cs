using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Pengaturan Gerak")]
    public float speed = 7f;
    public float jumpForce = 12f;

    [Header("Deteksi Lantai")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Pengaturan Serangan")]
    public GameObject punchHitbox;
    public float attackDuration = 0.2f;
    private bool isAttacking = false;

    [Header("Pengaturan Health")]
    public int maxHealth = 3;
    public int currentHealth;
    public float invincibleTime = 1f;
    private bool isInvincible = false;

    [Header("Pengaturan Respawn")]
    public Transform spawnPoint;
    public float respawnDelay = 2f;

    [Header("Referensi")]
    private Rigidbody2D rb;
    private SpriteRenderer[] spriteRenderers;
    private Collider2D[] playerColliders;
    private Animator animator;

    private float moveInput;
    private bool jumpRequest;

    private float startScaleX;
    private int faceDirection = 1; // 1 = kanan, -1 = kiri

    private bool isDead = false;
    private Vector3 spawnPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        playerColliders = GetComponentsInChildren<Collider2D>();

        currentHealth = maxHealth;

        startScaleX = Mathf.Abs(transform.localScale.x);

        if (spawnPoint != null)
        {
            spawnPosition = spawnPoint.position;
        }
        else
        {
            spawnPosition = transform.position;
        }

        if (punchHitbox != null)
        {
            punchHitbox.SetActive(false);
        }
    }

    void Update()
    {
        if (isDead) return;

        // Ambil input kiri/kanan
        moveInput = Input.GetAxisRaw("Horizontal");

        // Simpan arah hadap
        if (moveInput > 0)
        {
            faceDirection = 1;
        }
        else if (moveInput < 0)
        {
            faceDirection = -1;
        }

        // Ambil input lompat
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequest = true;
        }

        // Serang
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        CheckGrounded();

        // Gerak horizontal
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Lompat
        if (jumpRequest)
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            jumpRequest = false;
        }
    }

    void LateUpdate()
    {
        // LateUpdate berjalan setelah Animator, jadi arah tidak ditimpa Animator.
        Vector3 scale = transform.localScale;
        scale.x = startScaleX * faceDirection;
        transform.localScale = scale;
    }

    private void CheckGrounded()
    {
        if (groundCheck == null) return;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        if (punchHitbox != null)
        {
            punchHitbox.SetActive(true);
        }

        yield return new WaitForSeconds(attackDuration);

        if (punchHitbox != null)
        {
            punchHitbox.SetActive(false);
        }

        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Musuh") && !isInvincible)
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Duri") && !isInvincible)
        {
            TakeDamage(1);
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Heal! Darah sekarang: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || isDead) return;

        currentHealth -= damage;
        Debug.Log("Player kena damage! Sisa darah: " + currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(RespawnRoutine());
            return;
        }

        StartCoroutine(DamageBlink());
        StartCoroutine(InvincibleRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        isDead = true;
        isInvincible = true;
        moveInput = 0f;
        jumpRequest = false;
        isAttacking = false;

        if (punchHitbox != null)
        {
            punchHitbox.SetActive(false);
        }

        // Hentikan gerakan player
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        // Matikan collider sementara supaya tidak kena damage terus
        foreach (Collider2D col in playerColliders)
        {
            if (col != null)
            {
                col.enabled = false;
            }
        }

        // Jalankan animasi mati
        if (animator != null)
        {
            animator.SetTrigger("mati");
        }

        // Tunggu animasi mati
        yield return new WaitForSeconds(respawnDelay);

        // Pindahkan ke spawn
        transform.position = spawnPosition;

        // Reset darah dan status
        currentHealth = maxHealth;
        isDead = false;
        isInvincible = false;

        // Hidupkan physics lagi
        if (rb != null)
        {
            rb.simulated = true;
            rb.velocity = Vector2.zero;
        }

        // Hidupkan collider lagi
        foreach (Collider2D col in playerColliders)
        {
            if (col != null)
            {
                col.enabled = true;
            }
        }

        // Pastikan sprite kelihatan lagi
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                sr.enabled = true;
            }
        }

        // Reset animator agar tidak nyangkut di animasi mati
        if (animator != null)
        {
            animator.ResetTrigger("mati");
            animator.Rebind();
            animator.Update(0f);
        }

        Debug.Log("Player respawn ke titik awal!");
    }

    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    private IEnumerator DamageBlink()
    {
        int blinkCount = 4;

        for (int i = 0; i < blinkCount; i++)
        {
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                if (sr != null) sr.enabled = false;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (SpriteRenderer sr in spriteRenderers)
            {
                if (sr != null) sr.enabled = true;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}