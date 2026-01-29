using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")] //Stats del enemigo
    [SerializeField]
    private float health;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float detectionRange; //Este ser el X y el Z del area de deteccion
    [SerializeField]
    private float attackRange; //Rango de ataque y de stopping distance
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float basicAttackDMG; //Daño del ataque básico

    [Header("ThisEnemy")] //Cosas de este enemigo en concreto
    [SerializeField]
    private Transform[] patrolPoints; //Puntos de patrulla
    private int patrolIndex = 0;

    [Header("Things")] //Referencias a otros objetos y mas
    [SerializeField]
    private GameObject detectionArea;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform attackPivot;

    private bool DetectingPlayer = false;
    private bool onAttackingRange = true;
    private float timePass = 0;

    void Start()
    {
        detectionArea.transform.localScale = new Vector3(detectionRange, 3.2f, detectionRange);
        GetComponent<NavMeshAgent>().speed = speed;
        attackPivot.localScale = new Vector3(attackPivot.localScale.x,attackPivot.localScale.y,attackRange);
    }

    void TakeDamage(float amount) //Metodo para recibir daño
    {
        health -= amount;
        if (health <= 0)
        {
            // El enemigo muere
        }
        else 
        {
            //Sonido y efecto de daño
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Si el jugador entra en el area de deteccion hara que el booleano (DetectingPlayer) sea True
        if (other.gameObject == player)
        {
            DetectingPlayer = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        //Si el jugador sale del area de deteccion hara que el booleano (DetectingPlayer) sea False
        if (other.gameObject == player)
        {
            DetectingPlayer = false;
        }
    }

    void Update()
    {
        if (DetectingPlayer)
        {
            GetComponent<NavMeshAgent>().stoppingDistance = attackRange;
            transform.LookAt(player.transform); //El enemigo mira hacia el jugador cuando lo persigue
            GetComponent<NavMeshAgent>().SetDestination(player.transform.position); //El enemigo se movera hacia la posicion del jugador
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                //En rango de ataque
                if (timePass >= attackCooldown) //Esto es para el cooldown de ataque
                {
                    Camera.main.GetComponent<FollowMouse>().TakeDamage(basicAttackDMG); //El jugador recibe daño del ataque básico   ||  HAY QUE CAMBIARLO MÁS ADELANTE YA QUE LA VIDA DEL PLAYER SE MOVERÁ A OTRO SCRIPT
                    Debug.Log("ataque");

                    timePass = 0;
                }
                timePass += 1 * Time.deltaTime;
            }
        }
        else 
        {
            GetComponent<NavMeshAgent>().stoppingDistance = 0;
            if (patrolPoints.Length > 0)
            {
                //Patrulla entre los puntos de patrulla en orden
                GetComponent<NavMeshAgent>().SetDestination(patrolPoints[patrolIndex].position);
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

    //Falta el ataque básico al jugador cuando esté cerca, para ataques más especificos deberán hacerse en los scripts hijos que hereden de este
}
