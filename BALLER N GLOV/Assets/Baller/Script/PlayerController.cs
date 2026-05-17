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
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        
        currentHealth = maxHealth;

        if (punchHitbox != null) 
        {
            punchHitbox.SetActive(false);
        }
    }

    void Update()
    {
        // 1. JALAN & FLIP
        float moveInput = Input.GetAxisRaw("Horizontal"); 
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);

        // 2. LOMPAT
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false; 
        }

        // 3. MENONJOK
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        punchHitbox.SetActive(true);  

        yield return new WaitForSeconds(attackDuration); 

        punchHitbox.SetActive(false); 
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lantai")) 
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Musuh") && !isInvincible) 
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Duri") && !isInvincible)
        {
            TakeDamage(1);
        }
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
            spriteRenderer.enabled = false; 
            yield return new WaitForSeconds(0.1f); 
            spriteRenderer.enabled = true;  
            yield return new WaitForSeconds(0.1f); 
        }
    }
}