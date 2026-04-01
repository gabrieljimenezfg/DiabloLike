using UnityEngine;

public class LeverScript : MonoBehaviour
{
    [SerializeField] private GameObject destroy;
    private Animator animator;
    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX(audioClip,transform.position);
            animator.SetTrigger("Pull");
            destroy.SetActive(false);
        }
    }
}
