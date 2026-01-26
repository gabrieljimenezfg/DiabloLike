using UnityEngine;
using UnityEngine.AI;

public class FollowMouse : MonoBehaviour
{
    [SerializeField]
    private Transform followerObject; //el transform del objeto que seguira al mouse

    [SerializeField]
    private GameObject Player; //el jugador

    private Camera camera;


    void Start()
    {
        camera = Camera.main; //se asigna la camara a la variable 'camera'
    }


    void Update()
    {

        //  FOLLOW MOUSE ON GROUND
        //----------------------------------------------------------------------------------------
        Ray ray = camera.ScreenPointToRay(Input.mousePosition); //crea un rayo desde la camara hasta la posicion del mouse
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Ground")
            {
                followerObject.position = hit.point; //cuando el raycast golpea el suelo le asigna la posicion del contacto del raycast contra el suelo (hit.point) al followerObject
                                                     //con esto se hace que followerObject esté siempre en el suelo, aunque haya inclinación, elevación o bajadas en el suelo
            }
        }



        //  MOVER PLAYER HACIA FOLLOWEROBJECT CUANDO SE HAGA CLICK DERECHO
        //----------------------------------------------------------------------------------------
        if (Input.GetButtonDown("Fire2")) // Fire1 = Click Izquierdo   Fire2 = Click Derecho   Fire3 = Click Rueda
        {
            Player.GetComponent<NavMeshAgent>().SetDestination(followerObject.position); //Se asigna la destinacion del NavMeshAgent del Player a la posición del followerObject
        }


    }
}



//Problemas:

//      1) Si hay objetos entre la camara y el suelo que no tienen la tag "Ground" el raycast los golpea primero a ellos y no al suelo
//         hace que followerObject no se mueva, hay que buscar alguna forma de ignorar esos objetos
