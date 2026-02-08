using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float distance; //Maxima distancia que recorre antes de destruirse

    private Vector3 originalPos;
    private void Awake()
    {
        originalPos = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(originalPos, transform.position) > distance)
        {
            Destroy(gameObject); //Destruye el proyectil si ha recorrido (distance) y no ha colisionado contra nada
        }
    }

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
