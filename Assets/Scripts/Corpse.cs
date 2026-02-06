using System;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private float aliveTime;

    private void Start()
    {
        Destroy(gameObject, aliveTime);
    }

    public void SpawnMinion()
    {
        Instantiate(minionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}