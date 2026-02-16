using System;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private float aliveTime;

    private GameObject thisEnemy;

    private void Start()
    {
        Destroy(gameObject, aliveTime);
    }

    public void SpawnMinion()
    {
        Instantiate(minionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SetEnemy(GameObject enemy)
    {
        thisEnemy = enemy;
    }

    public void Revive()
    {
        thisEnemy.SetActive(true);
        thisEnemy.GetComponent<Enemy>().isAlive = true;
        thisEnemy.GetComponent<Enemy>().health = thisEnemy.GetComponent<Enemy>().maxHealth / 2;
        Destroy(gameObject);
    }
}