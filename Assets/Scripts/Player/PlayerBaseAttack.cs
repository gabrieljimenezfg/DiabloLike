using System;
using JetBrains.Annotations;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerBaseAttack : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDistance;
    [SerializeField] private float rangeBuffer = 0.25f;
    private Player player;
    private PlayerMovementController playerMovement;
    private Transform pendingTarget;

    public event EventHandler<GameObject> BaseAttackCasted;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovementController>();
    }

    private void Start()
    {
        GameInput.Instance.BaseAttackPerformed += OnBaseAttackPerformed;
        playerMovement.ManualMovePerformed += PlayerMovementOnManualMovePerformed;
    }

    private void PlayerMovementOnManualMovePerformed(object sender, EventArgs e)
    {
        pendingTarget = null;
    }

    private void OnDestroy()
    {
        GameInput.Instance.BaseAttackPerformed -= OnBaseAttackPerformed;
        playerMovement.ManualMovePerformed -= PlayerMovementOnManualMovePerformed;
    }

    private void OnBaseAttackPerformed(object sender, EventArgs e)
    {
        HandleBaseAttack();
    }

    private void Update()
    {
        if (!pendingTarget) return;
        if (!pendingTarget.gameObject.activeInHierarchy)
        {
            pendingTarget = null;
            return;
        }

        if (player.ArePositionAndRotationLocked) return;

        float dist = Vector3.Distance(player.transform.position, pendingTarget.position);

        if (dist <= projectileDistance)
        {
            playerMovement.Stop();
            BaseAttackCasted?.Invoke(this, pendingTarget.gameObject);
            pendingTarget = null;
            return;
        }

        playerMovement.MoveTo(pendingTarget.position);
    }


    private void HandleBaseAttack()
    {
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Enemy, out var hit))
        {
            var distanceToEnemy = hit.transform.position - player.transform.position;
            if (distanceToEnemy.magnitude <= projectileDistance)
            {
                playerMovement.Stop();
                BaseAttackCasted?.Invoke(this, hit.collider.gameObject);
            }
            else
            {
                pendingTarget = hit.transform;
            }
        }
    }

    public void LaunchProjectile(GameObject enemy)
    {
        var directionTowardsEnemy = (enemy.transform.position - transform.position).normalized;
        player.SetLookDirection(directionTowardsEnemy);
        var projectile = Instantiate(projectilePrefab, player.CastSpawnPoint.position,
            player.CastSpawnPoint.rotation);
        projectile.distance = projectileDistance;
        projectile.GetComponent<Rigidbody>().linearVelocity = player.CastSpawnPoint.forward * projectileSpeed;
    }
}