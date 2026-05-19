using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Pengaturan Gerak")]
    public float speed = 7f;
    public float jumpForce = 12f;

    [Header("Deteksi Lantai")]
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

    [Header("Referensi")]
    public Transform backgroundRotate;
    public float rotasiSpeed = 200f;

    private Rigidbody2D rb;
    private SpriteRenderer[] spriteRenderers;
    private bool isFacingRight = true;
    private PlayerAnimator playerAnimator;

    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        rb = GetComponent<Rigidbody2D>();

        // Mengambil semua SpriteRenderer dari player dan child-nya
        // Contoh: badan, mata, pupil, dan asset visual lain
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        currentHealth = maxHealth;

        if (punchHitbox != null)
        {
            punchHitbox.SetActive(false);
        }
    }

    void Update()
    {
        // 1. JALAN
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // 2. ROTASI BACKGROUND / BOLA
        if (moveInput != 0 && backgroundRotate != null)
        {
            float rotasiArah = moveInput > 0 ? -1f : 1f;
            backgroundRotate.Rotate(0f, 0f, rotasiArah * rotasiSpeed * Time.deltaTime);
        }

        // 3. FLIP ARAH SERANGAN
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // 4. LOMPAT
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;

            if (playerAnimator != null)
            {
                playerAnimator.TriggerLompat();
            }
        }

        // 5. MENYERANG
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        // Membalik posisi hitbox serangan ke arah player menghadap
        if (punchHitbox != null)
        {
            Vector3 pos = punchHitbox.transform.localPosition;
            punchHitbox.transform.localPosition = new Vector3(-pos.x, pos.y, pos.z);
        }
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
        // Menyentuh lantai
        if (collision.gameObject.CompareTag("Lantai"))
        {
            isGrounded = true;
        }

        // Menyentuh musuh
        if (collision.gameObject.CompareTag("Musuh") && !isInvincible)
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Menyentuh duri / trap / shuriken kalau pakai tag Duri
        if (collision.gameObject.CompareTag("Duri") && !isInvincible)
        {
            TakeDamage(1);
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Heal! Darah sekarang: " + currentHealth);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player kena damage! Sisa darah: " + currentHealth);

        StartCoroutine(DamageBlink());
        StartCoroutine(InvincibleRoutine());

        if (currentHealth <= 0)
        {
            Debug.Log("Player mati!");
            gameObject.SetActive(false);
        }
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
            // Matikan semua sprite visual player
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                if (sr != null)
                {
                    sr.enabled = false;
                }
            }

            yield return new WaitForSeconds(0.1f);

            // Hidupkan semua sprite visual player
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                if (sr != null)
                {
                    sr.enabled = true;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}