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
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        
        if (punchHitbox != null) punchHitbox.SetActive(false);
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

        // 3. MENONJOK (Ganti ke Klik Mouse Kiri)
        // GetMouseButtonDown(0) adalah fungsi untuk mendeteksi klik kiri mouse
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
        if (collision.gameObject.CompareTag("Lantai")) isGrounded = true;
        if (collision.gameObject.CompareTag("Musuh")) StartCoroutine(DamageBlink()); 
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