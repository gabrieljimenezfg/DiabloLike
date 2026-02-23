using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


public class ArcherEnemy : Enemy
{
    [SerializeField] private float reviveArea;
    [SerializeField] private float minEnemiesToRevive;
    [SerializeField] private float reviveCooldown;
    private bool reviveFlag = false; //Si esto es true significa que esta reviviendo a alguien

    public event EventHandler StartReviving;

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();
        if (FindCorpses(out List<GameObject> corpseList).Count >= minEnemiesToRevive)
        {
            if (!reviveFlag)
            {
                StartReviving?.Invoke(this, EventArgs.Empty);
                StartCoroutine(ReviveCooldown(corpseList));
            }
        }
    }

    IEnumerator ReviveCooldown(List<GameObject> cList)
    {
        reviveFlag = true;
        foreach (var corpse in cList)
        {
            yield return new WaitForSeconds(reviveCooldown);
            corpse.GetComponent<Corpse>().Revive();
        }
        reviveFlag = false;
    }

    private List<Enemy> FindEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, reviveArea);

        List<Enemy> inAreaEnemies = new List<Enemy>();

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<Enemy>(out Enemy enemy))
            {
                inAreaEnemies.Add(enemy);
            }
        }
        return inAreaEnemies;
    }

    private List<GameObject> FindCorpses(out List<GameObject> corpseList)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, reviveArea);

        List<GameObject> inAreaEnemies = new List<GameObject>();

        foreach (Collider col in colliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Corpse"))
            {
                inAreaEnemies.Add(col.gameObject);
            }
        }
        corpseList = inAreaEnemies;
        return inAreaEnemies;
    }
}
