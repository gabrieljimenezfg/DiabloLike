using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;

public class FollowMouse : MonoBehaviour
{
    //Mover mas tarde a otro script VVV
    [SerializeField] private float stamina;
    //Mover mas tarde a otro script ^^^

    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rollDistance;
    [SerializeField] private float rollSpeed;

    private bool lowerStamina = false; //Variable para controlar si se debe reducir la stamina o no
    private bool isMoving = false;
    private bool isRunning;

    [Header("BaseAttack")] [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDistance;

    [SerializeField]
    private Material outline; //Material de outline que se aplicara al enemigo cuando el mouse este sobre el

    [Header("Camera")]  private Camera camera;
    [SerializeField] private Vector3 camOffset;
    [SerializeField] private float camRunZoom; //Cuando el jugador corre la camara hara un pequeno zoom in

    [SerializeField]
    private float zoomSpeed = 5f; //Velocidad a la que la camara se mueve cuando se hace zoom in o zoom out

    private float camOriginalZoom;

    private GameObject currentHoveredEnemy;
    private Material[] originalMaterials;

    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        camera = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        camOriginalZoom = camOffset.y;
    }

    private void Start()
    {
        GameInput.Instance.MovementPerformed += GameInputOnMovementPerformed;
        GameInput.Instance.RunPerformed += GameInputOnRunPerformed;
        GameInput.Instance.BaseAttackPerformed += GameInputOnBaseAttackPerformed;
    }

    private void GameInputOnBaseAttackPerformed(object sender, EventArgs e)
    {
        HandleBaseAttack();
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
        //  IS THE MOUSE TOUCHING AN ENEMY?
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Enemy, out var enemyHit))
        {
            var enemyObj = enemyHit.collider.gameObject;
            if (currentHoveredEnemy != enemyObj) // El mouse ha pasado a estar sobre un nuevo enemigo
            {
                DeleteOutliner();
                currentHoveredEnemy = enemyObj;

                //Seguro hay alguna forma mas optima   V V V
                MeshRenderer mr = currentHoveredEnemy.GetComponent<MeshRenderer>();
                originalMaterials = mr.materials;
                Material[] newMats = new Material[originalMaterials.Length + 1];
                originalMaterials.CopyTo(newMats, 0);
                newMats[newMats.Length - 1] = outline;
                mr.materials = newMats;

                enemyObj.GetComponent<MeshRenderer>().materials = new Material[]
                    { enemyObj.GetComponent<MeshRenderer>().material, outline };
            }
        }
        else
        {
            // El mouse ya no esta sobre ningun enemigo
            DeleteOutliner();
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

    private void DeleteOutliner()
    {
        if (currentHoveredEnemy == null) return;

        MeshRenderer mr = currentHoveredEnemy.GetComponent<MeshRenderer>();
        mr.materials = originalMaterials;
        currentHoveredEnemy = null;
        originalMaterials = null;
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

    public void HandleMovement()
    {
        isMoving = true;
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Ground, out var groundHit))
        {
            navMeshAgent.SetDestination(groundHit.point);
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

    void LateUpdate()
    {
        camera.transform.position =
            new Vector3(transform.position.x, transform.position.y, transform.position.z) +
            camOffset; //La camara sigue al jugador en X y Z 

        camera.transform.LookAt(transform.position);

        //CAMERA ZOOM MANAGEMENT
        float currentZoom;
        if (isRunning)
        {
            currentZoom = camRunZoom;
        }
        else
        {
            currentZoom = camOriginalZoom;
        }

        camOffset.y =
            Mathf.Lerp(camOffset.y, currentZoom,
                Time.deltaTime *
                zoomSpeed); //Mueve la camara suavemente entre su posicion actual y la posici�n requerida (currentZoom)
    }

    public void HandleBaseAttack()
    {
        if (currentHoveredEnemy == null) return;
        
        Debug.Log("Atacando a " + currentHoveredEnemy.name);

        projectileSpawnPoint.LookAt(currentHoveredEnemy.transform
            .position); //Ajusta la direccion del spawn del proyectil hacia el enemigo
        GameObject projectileCopy = Instantiate(projectilePrefab, projectileSpawnPoint.position,
            projectileSpawnPoint.rotation);
        projectileCopy.GetComponent<Rigidbody>().linearVelocity =
            projectileSpawnPoint.forward *
            projectileSpeed; //Asigna velocidad al proyectil hacia la direccion del spawn
    }
}