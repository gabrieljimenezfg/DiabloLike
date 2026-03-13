using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject projectileDespawnPrefab;

    public float damage;
    public float maxDamage;
    public float distance; //Maxima distancia que recorre antes de destruirse

    public bool
        isGood = true; //Si esto es true es un proyectil del jugador que ataca enemigos, si es false es un proyectil de un enemigo que ataca players
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
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        if (isGood)
        {
            Instantiate(projectileDespawnPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGood)
        {
            if (other.CompareTag("Break"))
            {
                other.GetComponent<Animator>().SetTrigger("Break");
            }
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                DestroyProjectile();
            }
        }
        else
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                DestroyProjectile();
            }
        }
    }
}