using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private PlayerMovementController playerMC;

    //TEMPORAL
    bool attacking;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMC = GetComponentInParent<PlayerMovementController>();

        GameInput.Instance.BaseAttackPerformed += OnBaseAttackPerformed;
        SkillSystem.CastedSkill += OnSkillPerformed;
    }

    private void Update()
    {
        HandleMovementAnimation();
        HandleRunningAnimation();

        //TEMPORAL
        if(attacking)
        {
            Invoke("StopAttack", 0.5f);
        }
    }

    private void HandleMovementAnimation()
    {
        animator.SetBool("isMoving", playerMC.IsMoving);
    }

    private void HandleRunningAnimation()
    {
        animator.SetBool("isRunning", playerMC.IsRunning);
    }

    private void OnBaseAttackPerformed(object sender, EventArgs e)
    {
        HandleAttackAnimation();
    }

    private void HandleAttackAnimation()
    {
        animator.SetBool("Attack", true);

        //TEMPORAL
        attacking = true;
    }

    //TEMPORAL
    private void StopAttack()
    {
        animator.SetBool("Attack", false);
        attacking = false;
    }

    private void OnSkillPerformed(object sender, int slotID)
    {
        HandleSpellAnimation(slotID);
    }

    private void HandleSpellAnimation(int _slotId)
    {
        _slotId += 1;
        animator.SetTrigger("Spell" +  _slotId);
    }
}
