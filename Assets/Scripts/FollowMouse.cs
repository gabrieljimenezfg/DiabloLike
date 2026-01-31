using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections;

public class FollowMouse : MonoBehaviour
{
    //Mover más tarde a otro script VVV
    [SerializeField]
    private float maxLife;
    [SerializeField]
    private float currentLife;
    [SerializeField]
    private float stamina;
    //Mover más tarde a otro script ^^^

    [SerializeField]
    private float speed;
    [SerializeField]
    private float runSpeed;
    private bool lowerStamina = false; //Variable para controlar si se debe reducir la stamina o no

    [SerializeField]
    private Transform followerObject; //el transform del objeto que seguira al mouse

    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Vector3 camOffset;
    [SerializeField]
    private float camRunZoom; //Cuando el jugador corre la cámara hará un pequeño zoom in
    private float camOriginalZoom;
    [SerializeField]
    private float ZoomSpeed = 0.02f; //Tiempo de espera entre cada paso de zoom in

    private int groundLayer;

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
        Ray ray = camera.ScreenPointToRay(Input.mousePosition); //crea un rayo desde la camara hasta la posicion del mouse
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            followerObject.position = hit.point; //cuando el raycast golpea el suelo le asigna la posicion del contacto del raycast contra el suelo (hit.point) al followerObject
                                                 //con esto se hace que followerObject este siempre en el suelo, aunque haya inclinacion, elevacion o bajadas en el suelo
        }
        if (lowerStamina) {
            stamina -= 15f * Time.deltaTime; //Reduce la stamina mientras se corre
            if (stamina <= 0f) {
                stamina = 0f;
                lowerStamina = false;
                GetComponent<NavMeshAgent>().speed = speed; //Vuelve a la velocidad normal del NavMeshAgent del Player
                camOffset.y = camOriginalZoom; //Vuelve al zoom original de la camara
            }
        }
        if (!lowerStamina) {
            stamina += 5f * Time.deltaTime; //Recupera la stamina cuando no se corre
            if (stamina > 100f) {
                stamina = 100f;
            }
        }
    }

    public void Run(InputAction.CallbackContext callback)
    {
        //Correr cuando se pulse shift
        if (callback.performed)
        {
            GetComponent<NavMeshAgent>().speed = runSpeed; //Aumenta la velocidad del NavMeshAgent del Player
            StartCoroutine(ZoomCameraIn()); //Inicia la corrutina para hacer zoom in en la camara
            lowerStamina = true; //Comienza a reducir la stamina
        }
        else if (callback.canceled)
        {
            GetComponent<NavMeshAgent>().speed = speed; //Vuelve a la velocidad normal del NavMeshAgent del Player
            camOffset.y = camOriginalZoom; //Vuelve al zoom original de la camara
        }
    }

    IEnumerator ZoomCameraIn()
    {
        while (camOffset.y > camRunZoom)
        {
            camOffset.y-=0.2f; //Hace zoom in poco a poco (0.2) mientras corre
            yield return new WaitForSeconds(ZoomSpeed);
        }
    }

    public void Movement(InputAction.CallbackContext callback)
    {
        //Mover cuando se pulse click derecho
        GetComponent<NavMeshAgent>().SetDestination(followerObject.position); //Se asigna la destinacion del NavMeshAgent del Player a la posición del followerObject
    }

    void LateUpdate()
    {
        camera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z)+camOffset; //La camara sigue al jugador en X y Z 
    }


    //Mover más tarde a otro script VVV

    public void TakeDamage(float amount) //Metodo para recibir daño
    {
        currentLife -= amount;
        if (currentLife <= 0)
        {
            // El jugador muere
        }
        else 
        {
            //Sonido y efecto de daño
        }
    }
    //Mover más tarde a otro script ^^^

}