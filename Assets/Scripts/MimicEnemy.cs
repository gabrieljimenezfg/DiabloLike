using UnityEngine;

public class MimicEnemy : Enemy
{
    [SerializeField] private float deepInGround; //que tan metido en el eje Y está en su estado de mimic
    [SerializeField] private GameObject mimicAsset;
    [SerializeField] private Transform mimicTransform;
    void Start()
    {
        base.Start();
        GameObject prop = Instantiate(mimicAsset, mimicTransform.position, mimicTransform.rotation);
        prop.transform.SetParent(transform);
        //transform.position.y = deepInGround;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
