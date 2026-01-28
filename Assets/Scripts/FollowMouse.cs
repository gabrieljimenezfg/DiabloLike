using UnityEngine;
using UnityEngine.AI;

public class FollowMouse : MonoBehaviour
{
    [SerializeField]
    private Transform followerObject; //el transform del objeto que seguira al mouse

    [SerializeField]
    private GameObject Player; //el jugador

    private Camera camera;

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
                                                     //con esto se hace que followerObject esté siempre en el suelo, aunque haya inclinación, elevación o bajadas en el suelo
        }



        //  MOVER PLAYER HACIA FOLLOWEROBJECT CUANDO SE HAGA CLICK DERECHO
        //----------------------------------------------------------------------------------------
        if (Input.GetButtonDown("Fire2")) // Fire1 = Click Izquierdo   Fire2 = Click Derecho   Fire3 = Click Rueda
        {
            Player.GetComponent<NavMeshAgent>().SetDestination(followerObject.position); //Se asigna la destinacion del NavMeshAgent del Player a la posición del followerObject
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(Player.transform.position.x - -0.934f, transform.position.y, Player.transform.position.z - -8.2f); //La camara sigue al jugador en X y Z 
    }
}

