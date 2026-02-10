using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private PlayerMovementController playerMovementController;
    private PlayerBaseAttack playerBaseAttack;
    private const string AnimatorAttackKey = "Attack";
    private const string AnimatorMovingKey = "isMoving";
    private const string AnimatorRunningKey = "isRunning";
    private const string AnimatorSpellKey = "Spell";
    private const string AnimatorDeathKey = "Death";
    private const string AnimatorRollKey = "Roll";

    private GameObject enemyObjective;

    //TEMPORAL
    bool attacking;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovementController = GetComponentInParent<PlayerMovementController>();
        playerBaseAttack = GetComponentInParent<PlayerBaseAttack>();

        Player.Instance.PlayerDied += OnPlayerDeath;
        SkillSystem.CastedSkill += OnCastedSkill;
        playerBaseAttack.BaseAttackCasted += OnBaseAttackCasted;
        playerMovementController.Rolling += OnRolling;
    }

    private void Update()
    {
        HandleMovementAnimation();
        HandleRunningAnimation();

        //TEMPORAL
        if(attacking)
        {
            Invoke(nameof(StopAttack), 0.5f);
        }
    }

    //Movimiento
    private void HandleMovementAnimation()
    {
        animator.SetBool(AnimatorMovingKey, playerMovementController.IsMoving);
    }

    private void HandleRunningAnimation()
    {
        animator.SetBool(AnimatorRunningKey, playerMovementController.IsRunning);
    }

    private void OnRolling(object sender, EventArgs e)
    {
        animator.SetTrigger(AnimatorRollKey);
    }

    private void OnBaseAttackCasted(object sender, GameObject enemy)
    {
        HandleAttackAnimation();
        enemyObjective = enemy;
    }

    public void FireProjectile()
    {
        playerBaseAttack.LaunchProjectile(enemyObjective);
    }

    private void HandleAttackAnimation()
    {
        animator.SetBool(AnimatorAttackKey, true);

        //TEMPORAL
        attacking = true;
    }

    private void OnCastedSkill(object sender, int slotID)
    {
        HandleSpellAnimation(slotID);
    }

    private void HandleSpellAnimation(int _slotId)
    {
        _slotId += 1;
        animator.SetTrigger(AnimatorSpellKey + _slotId);
    }

    private void OnPlayerDeath(object sender, EventArgs e)
    {
        animator.SetTrigger(AnimatorDeathKey);
    }

    //TEMPORAL
    private void StopAttack()
    {
        animator.SetBool(AnimatorAttackKey, false);
        attacking = false;
    }
}
