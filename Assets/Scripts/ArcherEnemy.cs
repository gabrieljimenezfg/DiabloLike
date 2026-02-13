using UnityEngine;
using System.Collections.Generic;

public class ArcherEnemy : Enemy
{
    [SerializeField] private float reviveArea;

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    //hay que cambiar esto, de momento indica el enemigo más cercano pero queremos que pase una lista de todos los enemigos dentro del area
    private List<Enemy> FindEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, reviveArea);

        List<Enemy> inAreaEnemies = new List<Enemy>();

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<Enemy>(out var enemy))
            {
                inAreaEnemies.Add(enemy);
            }
        }
        return inAreaEnemies;
    }

    private List<Enemy> FindCorpses()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, reviveArea);

        List<Enemy> inAreaEnemies = new List<Enemy>();

        foreach (Collider col in colliders)
        {
            if (col.gameObject.layer.ToString() == "Corpse")
            {
                //inAreaEnemies.Add(enemy);
            }
        }
        return inAreaEnemies;
    }
}
