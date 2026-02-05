using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections;

public class FollowMouse : MonoBehaviour
{
    //Mover mas tarde a otro script VVV
    [SerializeField]
    private float currentLife;
    [SerializeField]
    private float stamina;
    //Mover mas tarde a otro script ^^^

    [SerializeField]
    private float speed;
    [SerializeField]
    private float runSpeed;
    private bool lowerStamina = false; //Variable para controlar si se debe reducir la stamina o no
    private bool isMoving = false;
    private bool isRunning;

    [SerializeField]
    private Transform followerObject; //el transform del objeto que seguira al mouse

    [Header("BaseAttack")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private Material outline; //Material de outline que se aplicara al enemigo cuando el mouse este sobre el

    [Header("Camera")]
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Vector3 camOffset;
    [SerializeField]
    private float camRunZoom; //Cuando el jugador corre la camara hara un pequeno zoom in
    [SerializeField]
    private float zoomSpeed = 5f; //Velocidad a la que la camara se mueve cuando se hace zoom in o zoom out
    private float camOriginalZoom;

    private int groundLayer;

    private GameObject currentHoveredEnemy;
    private Material[] originalMaterials;

    void Awake()
    {
        camera = Camera.main; //se asigna la camara a la variable 'camera'
        groundLayer = LayerMask.GetMask("Ground"); //se le asigna a la capa "Ground" a groundLayer
        GetComponent<NavMeshAgent>().speed = speed; //Se asigna la velocidad del NavMeshAgent del Player
        camOriginalZoom = camOffset.y; //Se guarda el zoom original de la camara
}


    void Update()
    {
        //  FOLLOW MOUSE ON GROUND
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Ground, out var groundHit)) { 
            followerObject.position = groundHit.point;
        }

        //  IS THE MOUSE TOUCHING AN ENEMY?
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Enemy, out var enemyHit))
        {
            var enemyObj = enemyHit.collider.gameObject;
            if (currentHoveredEnemy != enemyObj)  // El mouse ha pasado a estar sobre un nuevo enemigo
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

                enemyObj.GetComponent<MeshRenderer>().materials = new Material[] { enemyObj.GetComponent<MeshRenderer>().material, outline };
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
            GetComponent<NavMeshAgent>().speed = speed;
        }
        if (lowerStamina)
        {
            stamina -= 25f * Time.deltaTime; //Resta stamina (25f * deltatime) cuando se esta corriendo (lowerStamina es verdadero)
            if (stamina < 0f)
                stamina = 0f;
        }
        if (!lowerStamina)
        {
            stamina += 5f * Time.deltaTime; //Recupera stamina (5f * deltatime) cuando no se esta corriendo (lowerStamina es falso)
            if (stamina > 100f)
                stamina = 100f;
        }
        if (isMoving)
        {
            if (!GetComponent<NavMeshAgent>().pathPending &&
                GetComponent<NavMeshAgent>().remainingDistance <= GetComponent<NavMeshAgent>().stoppingDistance &&
                GetComponent<NavMeshAgent>().velocity.sqrMagnitude == 0f)
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


    public void Run(InputAction.CallbackContext callback)
    {
        //Correr cuando se mantenga pulsado shift, se este moviendo y tenga stamina
        if (callback.performed && isMoving && stamina > 0f)
        {
            isRunning = true;
            lowerStamina = true;
            GetComponent<NavMeshAgent>().speed = runSpeed;
        }

        //Dejar de correr cuando se suelte shift
        if (callback.canceled)
        {
            isRunning = false;
            lowerStamina = false;
            GetComponent<NavMeshAgent>().speed = speed;
        }
    }

    public void Movement(InputAction.CallbackContext callback)
    {
        isMoving = true;
        //Mover cuando se pulse click derecho
        GetComponent<NavMeshAgent>().SetDestination(followerObject.position); //Se asigna la destinacion del NavMeshAgent del Player a la posicion del followerObject
    }

    void LateUpdate()
    {
        camera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) + camOffset; //La camara sigue al jugador en X y Z 

        camera.transform.LookAt(transform.position);
        
        //CAMERA ZOOM MANAGEMENT
        float currentZoom;
        if (isRunning) {
            currentZoom = camRunZoom;
        }
        else
        {
            currentZoom = camOriginalZoom;
        }
        camOffset.y = Mathf.Lerp(camOffset.y, currentZoom, Time.deltaTime * zoomSpeed); //Mueve la camara suavemente entre su posicion actual y la posici�n requerida (currentZoom)
    }

    public void BaseAttack(InputAction.CallbackContext callback)
    {
        //Atacar cuando se pulse click izquierdo
        if (callback.performed && currentHoveredEnemy != null)
        {
            //Aqui iria el codigo de ataque al enemigo (currentHoveredEnemy)
            Debug.Log("Atacando a " + currentHoveredEnemy.name);

            projectileSpawnPoint.LookAt(currentHoveredEnemy.transform.position); //Ajusta la direccion del spawn del proyectil hacia el enemigo
            GameObject projectileCopy = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            projectileCopy.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.forward * projectileSpeed; //Asigna velocidad al proyectil hacia la direccion del spawn
        }
    }
}