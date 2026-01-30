using UnityEngine;
using UnityEngine.AI;

public class FollowMouse : MonoBehaviour
{
    //Mover más tarde a otro script VVV
    [SerializeField]
    private float maxLife;
    [SerializeField]
    private float currentLife;
    //Mover más tarde a otro script ^^^

    [SerializeField]
    private Transform followerObject; //el transform del objeto que seguira al mouse

    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Vector3 camOffset;

    private int groundLayer;

    void Awake()
    {
        camera = Camera.main; //se asigna la camara a la variable 'camera'
        groundLayer = LayerMask.GetMask("Ground"); //se le asigna a la capa "Ground" a groundLayer
    }


    void Update()
    {

        //  FOLLOW MOUSE ON GROUND
        //----------------------------------------------------------------------------------------
        Ray ray = camera.ScreenPointToRay(Input.mousePosition); //crea un rayo desde la camara hasta la posicion del mouse
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
                followerObject.position = hit.point; //cuando el raycast golpea el suelo le asigna la posicion del contacto del raycast contra el suelo (hit.point) al followerObject
                                                     //con esto se hace que followerObject este siempre en el suelo, aunque haya inclinacion, elevacion o bajadas en el suelo
        }



        //  MOVER PLAYER HACIA FOLLOWEROBJECT CUANDO SE HAGA CLICK DERECHO
        //----------------------------------------------------------------------------------------
        if (Input.GetButtonDown("Fire2")) // Fire1 = Click Izquierdo   Fire2 = Click Derecho   Fire3 = Click Rueda
        {
            GetComponent<NavMeshAgent>().SetDestination(followerObject.position); //Se asigna la destinacion del NavMeshAgent del Player a la posición del followerObject
        }
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