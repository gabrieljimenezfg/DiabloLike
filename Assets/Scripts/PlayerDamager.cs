using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
