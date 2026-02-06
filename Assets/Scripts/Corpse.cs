using UnityEngine;

public class Corpse : MonoBehaviour
{
    [SerializeField] private GameObject minionPrefab;

    public void SpawnMinion()
    {
        Instantiate(minionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}