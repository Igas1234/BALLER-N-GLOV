using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    public void TriggerLompat()
    {
        animator.ResetTrigger("jalan");
        animator.SetTrigger("lompat");
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        bool isGrounded = playerController.isGrounded;

        animator.SetBool("Floating", !isGrounded);
        animator.SetBool("Idle", isGrounded && moveInput == 0);

        if (isGrounded && moveInput != 0)
            animator.SetTrigger("jalan");
        else
            animator.ResetTrigger("jalan");
    }
}