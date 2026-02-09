using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private PlayerMovementController playerMC;
    private const string AnimAttackKey = "Attack";
    private const string AnimMovingKey = "isMoving";
    private const string AnimRunningKey = "isRunning";
    private const string AnimSpellKey = "Spell";
    private const string AnimDeathKey = "Death";

    //TEMPORAL
    bool attacking;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMC = GetComponentInParent<PlayerMovementController>();

        GameInput.Instance.BaseAttackPerformed += OnBaseAttackPerformed;
        Player.Instance.PlayerDied += OnPlayerDeath;
        SkillSystem.CastedSkill += OnCastedSkill;
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

    private void HandleMovementAnimation()
    {
        animator.SetBool(AnimMovingKey, playerMC.IsMoving);
    }

    private void HandleRunningAnimation()
    {
        animator.SetBool(AnimRunningKey, playerMC.IsRunning);
    }

    private void OnBaseAttackPerformed(object sender, EventArgs e)
    {
        HandleAttackAnimation();
    }

    private void HandleAttackAnimation()
    {
        animator.SetBool(AnimAttackKey, true);

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
        animator.SetTrigger(AnimSpellKey + _slotId);
    }

    private void OnPlayerDeath(object sender, EventArgs e)
    {
        animator.SetTrigger(AnimDeathKey);
    }

    //TEMPORAL
    private void StopAttack()
    {
        animator.SetBool(AnimAttackKey, false);
        attacking = false;
    }
}
