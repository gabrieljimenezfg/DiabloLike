using System;
using UnityEngine;

public class PlayerBaseAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDistance;
    private Transform playerCastSpawnPoint;


    private void Start()
    {
        GameInput.Instance.BaseAttackPerformed += OnBaseAttackPerformed;
        playerCastSpawnPoint = GetComponent<Player>().CastSpawnPoint;
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

            LaunchProjectile(enemy);
        }
    }

    private void LaunchProjectile(GameObject enemy)
    {
        playerCastSpawnPoint.LookAt(enemy.transform.position);
        GameObject projectile = Instantiate(projectilePrefab, playerCastSpawnPoint.position,
            playerCastSpawnPoint.rotation);

        projectile.GetComponent<Rigidbody>().linearVelocity =
            playerCastSpawnPoint.forward *
            projectileSpeed;
    }
}