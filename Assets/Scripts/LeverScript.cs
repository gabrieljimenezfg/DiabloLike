using UnityEngine;

public class LeverScript : MonoBehaviour
{
    [SerializeField] private GameObject destroy;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("Pull");
            destroy.SetActive(false);
        }
    }
}
