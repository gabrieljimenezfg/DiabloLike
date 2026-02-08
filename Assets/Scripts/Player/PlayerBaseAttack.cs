using System;
using UnityEngine;

public class PlayerBaseAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDistance;
    

    public Transform ProjectileSpawnPoint => projectileSpawnPoint;
    
    private void Start()
    {
        GameInput.Instance.BaseAttackPerformed += OnBaseAttackPerformed;
    }

    private void OnDestroy()
    {
        GameInput.Instance.BaseAttackPerformed += OnBaseAttackPerformed;
    }

    private void OnBaseAttackPerformed(object sender, EventArgs e)
    {
        HandleBaseAttack();
    }

    private void HandleBaseAttack()
    {
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Enemy, out var hit))
        {
            var enemy = hit.collider.gameObject;

            //Ajusta la direccion del spawn del proyectil hacia el enemigo
            projectileSpawnPoint.LookAt(enemy.transform.position);
            GameObject projectileCopy = Instantiate(projectilePrefab, projectileSpawnPoint.position,
                projectileSpawnPoint.rotation);
            //Asigna velocidad al proyectil hacia la direccion del spawn
            projectileCopy.GetComponent<Rigidbody>().linearVelocity =
                projectileSpawnPoint.forward *
                projectileSpeed;
        }
    }
}