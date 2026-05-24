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

    private Rigidbody2D rb;
    private SpriteRenderer[] spriteRenderers;
    private bool isFacingRight = true;
    private PlayerAnimator playerAnimator;

    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        rb = GetComponent<Rigidbody2D>();

        // Mengambil semua SpriteRenderer dari player dan child-nya
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        currentHealth = maxHealth;

        if (punchHitbox != null)
        {
            punchHitbox.SetActive(false);

        isGrounded = false;
        }
    }

    void Update()
    {
        //Debug.Log("isGrounded: " + isGrounded);

        // 1. JALAN & FLIP
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput > 0 && !isFacingRight) Flip();
        else if (moveInput < 0 && isFacingRight) Flip();

        // 4. LOMPAT
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // 5. MENYERANG
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void Flip()
    {
        // Mengubah status arah hadap
        isFacingRight = !isFacingRight;

        // Membalikkan visual seluruh karakter (termasuk mata, pupil, dll)
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1; // Mengubah skala X menjadi minus untuk membalik gambar
        transform.localScale = characterScale;

        // Catatan: Karena punchHitbox biasanya ada di dalam Player (sebagai Child),
        // saat Player dibalik menggunakan transform.localScale di atas,
        // posisi hitbox otomatis ikut berbalik dengan sempurna.
        // Jadi kita tidak perlu lagi memindahkan manual posisi X hitbox-nya.
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
        // Menyentuh duri / trap / shuriken
        if (collision.gameObject.CompareTag("Duri") && !isInvincible)
        {
            TakeDamage(1);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lantai"))
            isGrounded = false;
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
}