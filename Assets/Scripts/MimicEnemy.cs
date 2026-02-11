using UnityEngine;

public class MimicEnemy : Enemy
{
    [SerializeField] private float deepInGround; //que tan metido en el eje Y está en su estado de mimic
    [SerializeField] private GameObject mimicAsset;
    [SerializeField] private Transform mimicTransform;

    private GameObject prop;
    private Collider collider;

    void Start()
    {
        base.Start();
        collider = GetComponent<Collider>();
        navMeshAgent.enabled = false;
        collider.enabled = false;
        prop = Instantiate(mimicAsset, mimicTransform.position, mimicTransform.rotation);
        prop.transform.SetParent(mimicTransform);
        prop.transform.localScale = Vector3.one;
        transform.position = new Vector3(transform.position.x, transform.position.y - deepInGround, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (isPlayerDetected && navMeshAgent.enabled == false)
        {
            navMeshAgent.enabled = true;
            collider.enabled = true;
            transform.position = new Vector3(transform.position.x, transform.position.y + deepInGround, transform.position.z);
            Destroy(prop);
        }
    }
}
