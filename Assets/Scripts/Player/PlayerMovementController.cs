using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float stamina;
    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rollDistance;
    [SerializeField] private float rollSpeed;

    private Player player;

    private bool lowerStamina = false; //Variable para controlar si se debe reducir la stamina o no
    private bool isMoving = false;
    private bool isRunning;

    [Header("Roll")]
    [SerializeField] private float rollStaminaConsumption;
    private bool isRolling = false;

    private NavMeshAgent navMeshAgent;

    public bool IsRunning => isRunning;
    public bool IsMoving => isMoving;

    void Awake()
    {
        player = GetComponent<Player>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        navMeshAgent.updateRotation = false;
    }

    private void Start()
    {
        GameInput.Instance.MovementPerformed += GameInputOnMovementPerformed;
        GameInput.Instance.RunPerformed += GameInputOnRunPerformed;
        GameInput.Instance.RollPerformed += GameInputOnRollPerformed;
    }

    private void GameInputOnMovementPerformed(object sender, EventArgs e)
    {
        HandleMovement();
    }

    private void GameInputOnRunPerformed(object sender, InputActionPhase e)
    {
        Run(e);
    }
    
    private void GameInputOnRollPerformed(object sender, EventArgs e)
    {
        Roll();
    }

    void Update()
    {
        if (player.ArePositionAndRotationLocked)
        {
            navMeshAgent.ResetPath();
            isMoving = false;
            player.SlowlyTurnTowardsMouse();
        }
        
        // STAMINA/RUN MANAGEMENT
        if (stamina <= 0f) //Si la stamina llega a 0 ya no puede correr mas
        {
            stamina = 0f;
            lowerStamina = false;
            isRunning = false;
            navMeshAgent.speed = speed;
        }

        if (lowerStamina)
        {
            stamina -= 25f *
                       Time.deltaTime; //Resta stamina (25f * deltatime) cuando se esta corriendo (lowerStamina es verdadero)
            if (stamina < 0f)
                stamina = 0f;
        }

        if (!lowerStamina)
        {
            stamina += 5f *
                       Time.deltaTime; //Recupera stamina (5f * deltatime) cuando no se esta corriendo (lowerStamina es falso)
            if (stamina > 100f)
                stamina = 100f;
        }

        if (isMoving)
        {
            if (!navMeshAgent.pathPending &&
                navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
                navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                lowerStamina = false;
                isMoving = false;
            }
        }
    }

    public void Run(InputActionPhase phase)
    {
        //Correr cuando se mantenga pulsado shift, se este moviendo y tenga stamina
        if (phase == InputActionPhase.Performed && isMoving && stamina > 0f)
        {
            isRunning = true;
            lowerStamina = true;
            navMeshAgent.speed = runSpeed;
        }

        //Dejar de correr cuando se suelte shift
        if (phase == InputActionPhase.Canceled)
        {
            isRunning = false;
            lowerStamina = false;
            navMeshAgent.speed = speed;
        }
    }

    private void HandleMovement()
    {
        isMoving = true;
        if (player.ArePositionAndRotationLocked) return;

        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Ground, out var groundHit))
        {
            var destination = groundHit.point;
            var direction = (destination - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            navMeshAgent.SetDestination(destination);
        }
    }
    
    
    public void Roll()
    {
        if (!isRolling && stamina >= rollStaminaConsumption)
        {
            stamina -= rollStaminaConsumption; //Resta la cantidad de stamina que consume el roll
            StartCoroutine(RollCoroutine());
        }
    }

    IEnumerator RollCoroutine() //Corrutina de roll
    {
        if (!MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Ground, out var groundHit))
        {
            yield break; //Si no se pudo obtener la posicion del mouse en el suelo, se sale de la corrutina
        }
        isRolling = true;
        navMeshAgent.ResetPath(); //Resetea el path del NavMeshAgent para que no intente seguir el camino mientras se esta haciendo el roll
        Player.Instance.invincible = true; //se vuelve invencible al iniciar el roll
        GetComponent<Animator>().SetTrigger("Roll");
        Vector3 targetPosition;

        transform.LookAt(groundHit.point); //Hace que el jugador mire hacia el followerObject (hacia donde se hizo click derecho) antes de hacer el roll

        // Calculacion de la posicion a donde se va a hacer el roll   V V V
        Vector3 rollDirection = (groundHit.point - transform.position).normalized;
        targetPosition = transform.position + rollDirection * (rollDistance);
        targetPosition.y = transform.position.y; //Se mantiene la misma altura para evitar que el jugador se eleve o se hunda durante el roll

        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit targetPosInNavMesh, 2f, LayerMask.GetMask("Ground"))) //NavMesh.SamplePosition pone a targetPosition (nuestro destino) en el punto mas cercano dentro del navmesh para evitar que el jugador intente rodar hacia un punto que no se pueda alcanzar (como una pared o un precipicio)
        {
            targetPosition = targetPosInNavMesh.position;
        }

        Vector3 startPos = transform.position; //Se guarda la posicion de antes de hacer el roll, es necesario para el lerp de mas abajo
        float t = 0f; //Variable (del 0 al 1) que se usa para saber cuanto le falta para terminar de moverse al targetPosition
        while (t < 1f)
        {
            t += Time.deltaTime * rollSpeed; //Se incrementa 't'
            Vector3 nextPos = Vector3.Lerp(startPos, targetPosition, t); //posicion inicial, posicion final, tiempo
            navMeshAgent.Move(nextPos - transform.position);
            yield return null; //Espera al siguiente frame
        }
        Player.Instance.invincible = false;
        isRolling = false;
    }

    public void ReactivateDamage()
    {
        //Esto es para el evento del rol en el que se reactivará su danyo
        Player.Instance.invincible = false;
    }
}