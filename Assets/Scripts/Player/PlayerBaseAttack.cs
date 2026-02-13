using System;
using JetBrains.Annotations;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerBaseAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDistance;
    private Player player;

    public event EventHandler<GameObject> BaseAttackCasted;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

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
            BaseAttackCasted?.Invoke(this, hit.collider.gameObject);
            //var enemy = hit.collider.gameObject;

            //LaunchProjectile(enemy);
        }
    }

    public void LaunchProjectile(GameObject enemy)
    {
        var directionTowardsEnemy = (enemy.transform.position - transform.position).normalized;
        player.SetLookDirection(directionTowardsEnemy);
        var projectile = Instantiate(projectilePrefab, player.CastSpawnPoint.position,
            player.CastSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().linearVelocity = player.CastSpawnPoint.forward * projectileSpeed;
    }
}