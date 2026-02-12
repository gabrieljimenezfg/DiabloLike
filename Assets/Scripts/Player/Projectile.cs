using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float distance; //Maxima distancia que recorre antes de destruirse
    public bool isGood = true; //Si esto es true es un proyectil del jugador que ataca enemigos, si es false es un proyectil de un enemigo que ataca players
                                //Se setea desde el script del enemigo

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
        if (isGood)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject); // Destruye el proyectil al impactar con un enemigo
            }
        }
        else
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject); // Destruye el proyectil al impactar con el jugador 
            }
        }
    }
}
