using UnityEngine;

public class AutoDestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float destroyAfterSeconds = 2f;

    void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }
}