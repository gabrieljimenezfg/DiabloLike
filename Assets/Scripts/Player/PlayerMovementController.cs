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

    private NavMeshAgent navMeshAgent;

    public bool IsRunning => isRunning;

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
    }

    private void GameInputOnMovementPerformed(object sender, EventArgs e)
    {
        HandleMovement();
    }

    private void GameInputOnRunPerformed(object sender, InputActionPhase e)
    {
        Run(e);
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

    /*
    public void Roll(InputAction.CallbackContext callback)
    {
        if (callback.performed == true) {
            Player.Instance.invincible = true;
            GetComponent<Animator>().SetTrigger("Roll"); // Que haga la animación de roll y que al final haya un evento que active tu hitbox denuevo
            if (isMoving)
            {
                Vector3 rollDirection = (followerObject.position - transform.position).normalized;
                Vector3 targetPosition = transform.position + rollDirection * rollDistance;
            }
            else {
                Vector3 targetPosition = transform.position + transform.forward * rollDistance;
            }
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, rollSpeed);
            }
        }
    }
    */

    public void ReactivateDamage()
    {
        //Esto es para el evento del rol en el que se reactivará su danyo
        Player.Instance.invincible = false;
    }
}