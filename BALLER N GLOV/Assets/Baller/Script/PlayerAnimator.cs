using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    private Rigidbody2D rb;
    private bool sudahLompat = false;
    private float maxHeightY = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
         if (playerController.currentHealth <= 0) return;
        float velocityY = rb.velocity.y;
        float moveInput = Input.GetAxisRaw("Horizontal");
        bool isGrounded = playerController.isGrounded;

        // Tracking posisi Y tertinggi saat di udara
        if (!isGrounded)
            maxHeightY = Mathf.Max(maxHeightY, transform.position.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            sudahLompat = true;

        // Jatuh biasa dari ketinggian cukup
        float jatuhThreshold = 3f; // sesuaikan, makin besar makin tinggi syaratnya
        if (!isGrounded && !sudahLompat && (transform.position.y < maxHeightY - jatuhThreshold))
            sudahLompat = true;

        // Reset
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Jatuh") && stateInfo.normalizedTime >= 0.9f)
        {
            sudahLompat = false;
            maxHeightY = 0f;
        }
        if (stateInfo.IsName("Serang") && stateInfo.normalizedTime >= 0.9f)
            animator.ResetTrigger("serang");

        if (isGrounded) maxHeightY = 0f;

        if (Input.GetMouseButtonDown(0))
            animator.SetTrigger("serang");


        animator.SetFloat("velocityY", velocityY);
        animator.SetFloat("moveInput", Mathf.Abs(moveInput));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("sudahLompat", sudahLompat);
    }
}