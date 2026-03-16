using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")] //Stats del enemigo
    public float maxHealth;
    public float health;

    public float speed;
    public float damage;
    [SerializeField] private float detectionRange; //Este ser el X y el Z del area de deteccion
    public float attackRange; //Rango de ataque y de stopping distance
    public float attackCooldown;
    [SerializeField] private float basicAttackDMG; //Danyo del ataque basico
    [SerializeField] private float projectileVelocity;
    [SerializeField] private int potionDropChancePercentage = 40;

    [Header("ThisEnemy")] //Cosas de este enemigo en concreto
    [SerializeField] private AudioClip damageAudio;
    [SerializeField]
    private Transform[] patrolPoints; //Puntos de patrulla
    public bool IsMelee = true;
    [SerializeField] private GameObject attackPref;
    [SerializeField] private Corpse corpse;
    [SerializeField] private GameObject prize;

    [SerializeField] private AudioClip meleeAudio;
    [SerializeField] private AudioClip unmeleeAudio;
    public bool isAlive = true;

    private int patrolIndex = 0;

    [Header("Things")] //Referencias a otros objetos y mas
    [SerializeField]
    private GameObject detectionArea;

    private Player player;
    [SerializeField] private Transform attackPivot;

    public bool isPlayerDetected = false; //Es publico para que los enemigos lo hereden
    private bool onAttackingRange = true;
    private float cooldownTimer = 0;
    public NavMeshAgent navMeshAgent; //El NavMeshAgent es publico para que los enemigos lo hereden
    [SerializeField] private bool isFinalBoss;

    public event EventHandler StartAttacking;
    public event EventHandler StopAttacking;
    public event EventHandler StartPatrolling;
    public event EventHandler StartChasing;

    [SerializeField] private int skillToGet = 999;

    public void Start()
    {
        health = maxHealth;
        player = Player.Instance;
        detectionArea.transform.localScale = new Vector3(detectionRange, 3.2f, detectionRange);
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        attackPivot.localScale = new Vector3(attackPivot.localScale.x, attackPivot.localScale.y, attackRange);
    }

    public void TakeDamage(float amount) //Metodo para recibir danyo
    {
        if (!isPlayerDetected)
        {
            isPlayerDetected = true;
        }
        health -= amount;
        DamagePopup.Create(transform.position, amount);
        if (health <= 0)
        {
            AudioManager.instance.PlaySFX(damageAudio, transform.position);
            Die();
        }
        else
        {
            //
        }
    }

    private void Die()
    {
        isAlive = false;
        SpawnCorpse();
        TryDropPotion();
        if (prize != null)
        {
            Instantiate(prize, transform.position, Quaternion.identity);
        }
        if (isFinalBoss)
        {
            Player.Instance.playerWin = true;
        }
        player.GetComponent<SkillSystem>().AcquireNewSkill(skillToGet);
        gameObject.SetActive(false);
    }

    private void SpawnCorpse()
    {
        var spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Corpse corpseSpawned = Instantiate(corpse, spawnPosition, Quaternion.identity);
        corpseSpawned.SetEnemy(gameObject);
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

    public void Update()
    {
        if (isPlayerDetected)
        {
            navMeshAgent.stoppingDistance = attackRange;
            transform.LookAt(player.transform); //El enemigo mira hacia el jugador cuando lo persigue
            navMeshAgent.SetDestination(player.transform.position); //El enemigo se movera hacia la posicion del jugador
            StartChasing?.Invoke(this, EventArgs.Empty);
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                //En rango de ataque
                StartAttacking?.Invoke(this, EventArgs.Empty);
                if (cooldownTimer >= attackCooldown) //Esto es para el cooldown de ataque
                {
                    if (IsMelee)
                    {
                        AudioManager.instance.PlaySFX(meleeAudio, transform.position);
                        player.GetComponent<Player>().TakeDamage(basicAttackDMG); //El jugador recibe danyo del ataque basico 
                    } else {
                        Debug.Log("RANGED ATTACK EFFECTED");
                        AudioManager.instance.PlaySFX(unmeleeAudio, transform.position);
                        GameObject atkPref = Instantiate(attackPref, attackPivot.position, transform.rotation); //Instancia el prefab del ataque si es que tiene uno

                        //Se setea el proyectil V V V
                        atkPref.GetComponent<Projectile>().isGood = false; //El proyectil es malo porque es del enemigo
                        atkPref.GetComponent<Projectile>().damage = damage; //El proyectil hace el danyo que se le asigno al enemigo
                        atkPref.GetComponent<Projectile>().distance = attackRange; //El proyectil tiene un rango de ataque igual al del enemigo
                        atkPref.GetComponent<Projectile>().transform.forward = transform.forward; //El proyectil se mueve hacia adelante del enemigo
                        atkPref.GetComponent<Rigidbody>().linearVelocity = transform.forward * projectileVelocity; //El proyectil se mueve a una velocidad de 10 (esto se puede ajustar segun el ataque)
                    }
                    cooldownTimer = 0;
                }

                cooldownTimer += Time.deltaTime;
            }
            else
            {
                StopAttacking?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            navMeshAgent.stoppingDistance = 0;
            StartPatrolling?.Invoke(this, EventArgs.Empty);
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

        if(Player.Instance.HP <= 0)
        {
            isPlayerDetected = false;
        }
    }

    //Para ataques mas especificos deberian hacerse en los scripts hijos que hereden de este
}