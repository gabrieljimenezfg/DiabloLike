using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); // Destruye el proyectil al impactar con un enemigo
        }
    }
}
