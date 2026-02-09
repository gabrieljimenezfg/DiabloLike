using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMovement()
    {
        animator.SetBool("isMoving", true);
    }

    private void OnRunning()
    {
        animator.SetBool("isRunning", true);
    }
}
