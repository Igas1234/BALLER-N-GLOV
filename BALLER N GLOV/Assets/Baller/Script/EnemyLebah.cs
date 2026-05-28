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

    private Vector2 startPosition;
    private bool movingPositive = true;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
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

            if (animator != null)
            {
                animator.SetTrigger("mati");
            }

            StartCoroutine(MatiRoutine());
        }

        if (collision.gameObject.CompareTag("Player"))
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