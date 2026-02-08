using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")] //Stats del enemigo
    [SerializeField]
    private float health;

    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float detectionRange; //Este ser el X y el Z del area de deteccion
    [SerializeField] private float attackRange; //Rango de ataque y de stopping distance
    [SerializeField] private float attackCooldown;
    [SerializeField] private float basicAttackDMG; //Da�o del ataque b�sico

    [SerializeField] private int potionDropChancePercentage = 40;

    [Header("ThisEnemy")] //Cosas de este enemigo en concreto
    [SerializeField]
    private Transform[] patrolPoints; //Puntos de patrulla

    [SerializeField] private Corpse corpse;

    private int patrolIndex = 0;

    [Header("Things")] //Referencias a otros objetos y mas
    [SerializeField]
    private GameObject detectionArea;

    private Player player;
    [SerializeField] private Transform attackPivot;

    private bool isPlayerDetected = false;
    private bool onAttackingRange = true;
    private float cooldownTimer = 0;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        player = Player.Instance;
        detectionArea.transform.localScale = new Vector3(detectionRange, 3.2f, detectionRange);
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        attackPivot.localScale = new Vector3(attackPivot.localScale.x, attackPivot.localScale.y, attackRange);
    }

    public void TakeDamage(float amount) //Metodo para recibir da�o
    {
        health -= amount;
        DamagePopup.Create(transform.position, amount);
        if (health <= 0)
        {
            Die();
        }
        else
        {
            //Sonido y efecto de da�o
        }
    }

    private void Die()
    {
        SpawnCorpse();
        TryDropPotion();
        Destroy(gameObject);
    }

    private void SpawnCorpse()
    {
        var spawnPosition =
            new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Instantiate(corpse, spawnPosition, Quaternion.identity);
    }

    private void TryDropPotion()
    {
        if (!ShouldDropPotion()) return;

        var potionPrefab = GetRandomPotionPrefab();
        var potionDropOffset = GetRandomOffsetDirection();
        var dropPosition = transform.position += potionDropOffset;

        Instantiate(potionPrefab, dropPosition, Quaternion.identity);
    }

    private Transform GetRandomPotionPrefab()
    {
        return Random.value < 0.5f
            ? GameAssets.i.healingPotionPrefab
            : GameAssets.i.manaPotionPrefab;
    }

    private Vector3 GetRandomOffsetDirection()
    {
        return Random.value < 0.5f
            ? transform.right
            : -transform.right;
    }

    private bool ShouldDropPotion()
    {
        return Random.Range(0, 100) < potionDropChancePercentage;
    }

    void OnTriggerEnter(Collider other)
    {
        //Si el jugador entra en el area de deteccion hara que el booleano (DetectingPlayer) sea True
        if (other.TryGetComponent(out Player _))
        {
            isPlayerDetected = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Si el jugador sale del area de deteccion hara que el booleano (DetectingPlayer) sea False
        if (other.TryGetComponent(out Player _))
        {
            isPlayerDetected = false;
        }
    }

    void Update()
    {
        if (isPlayerDetected)
        {
            navMeshAgent.stoppingDistance = attackRange;
            transform.LookAt(player.transform); //El enemigo mira hacia el jugador cuando lo persigue
            navMeshAgent 
                .SetDestination(player.transform.position); //El enemigo se movera hacia la posicion del jugador
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                //En rango de ataque
                if (cooldownTimer >= attackCooldown) //Esto es para el cooldown de ataque
                {
                    player.GetComponent<Player>()
                        .TakeDamage(
                            basicAttackDMG); //El jugador recibe da�o del ataque b�sico   ||  HAY QUE CAMBIARLO M�S ADELANTE YA QUE LA VIDA DEL PLAYER SE MOVER� A OTRO SCRIPT

                    cooldownTimer = 0;
                }

                cooldownTimer += Time.deltaTime;
            }
        }
        else
        {
            navMeshAgent.stoppingDistance = 0;
            if (patrolPoints.Length > 0)
            {
                //Patrulla entre los puntos de patrulla en orden
                navMeshAgent.SetDestination(patrolPoints[patrolIndex].position);
                float distance = (patrolPoints[patrolIndex].position - transform.position).magnitude;
                if (distance < 1)
                {
                    patrolIndex++;
                    if (patrolIndex >= patrolPoints.Length)
                    {
                        patrolIndex = 0;
                    }
                }
            }
        }
    }

    //Para ataques m�s especificos deber�n hacerse en los scripts hijos que hereden de este
}