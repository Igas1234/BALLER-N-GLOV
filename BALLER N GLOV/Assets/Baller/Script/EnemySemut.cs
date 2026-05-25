using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySemut : MonoBehaviour
{
    [Header("Pengaturan Patroli")]
    public float speed = 3f;
    public float patrolDistance = 3f;

    private Vector2 startPosition;
    private bool movingRight = true;
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

        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= startPosition.x + patrolDistance)
                movingRight = false;
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= startPosition.x - patrolDistance)
                movingRight = true;
        }

        Vector3 scale = transform.localScale;
        scale.x = movingRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Serangan") && !isDead)
        {
            isDead = true;
            animator.SetTrigger("mati");
            StartCoroutine(MatiRoutine());
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
                player.TakeDamage(1);
        }
    }

    private IEnumerator MatiRoutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}