using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public class SkillPerformedEventArgs : EventArgs
    {
        public int slotId;
    }

    public event EventHandler MovementPerformed;
    public event EventHandler<InputActionPhase> RunPerformed;
    public event EventHandler<SkillPerformedEventArgs> SkillPerformed;
    public event EventHandler BaseAttackPerformed;
    public event EventHandler HealPotionUsed;
    public event EventHandler ManaPotionUsed;

    private PlayerInputActions playerInputActions;

    private Action<InputAction.CallbackContext> _slot1;
    private Action<InputAction.CallbackContext> _slot2;
    private Action<InputAction.CallbackContext> _slot3;
    private Action<InputAction.CallbackContext> _slot4;

    private void Awake()
    {
        Instance = this;


        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        _slot1 = _ => OnSkillPerformed(0);
        _slot2 = _ => OnSkillPerformed(1);
        _slot3 = _ => OnSkillPerformed(2);
        _slot4 = _ => OnSkillPerformed(3);

        playerInputActions.Player.SkillSlot1.performed += _slot1;
        playerInputActions.Player.SkillSlot2.performed += _slot2;
        playerInputActions.Player.SkillSlot3.performed += _slot3;
        playerInputActions.Player.SkillSlot4.performed += _slot4;

        playerInputActions.Player.Movement.performed += OnMovement;

        playerInputActions.Player.Run.performed += OnRun;
        playerInputActions.Player.Run.canceled += OnRun;

        playerInputActions.Player.BaseAttack.performed += OnBaseAttack;

        playerInputActions.Player.HealPotion.performed += OnHealPotion;
        playerInputActions.Player.ManaPotion.performed += OnManaPotion;
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        MovementPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void OnRun(InputAction.CallbackContext obj)
    {
        RunPerformed?.Invoke(this, obj.phase);
    }

    private void OnHealPotion(InputAction.CallbackContext obj)
    {
        HealPotionUsed?.Invoke(this, EventArgs.Empty);
    }

    private void OnManaPotion(InputAction.CallbackContext obj)
    {
        ManaPotionUsed?.Invoke(this, EventArgs.Empty);
    }

    private void OnBaseAttack(InputAction.CallbackContext obj)
    {
        BaseAttackPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        playerInputActions.Player.SkillSlot1.performed -= _slot1;
        playerInputActions.Player.SkillSlot2.performed -= _slot2;
        playerInputActions.Player.SkillSlot3.performed -= _slot3;
        playerInputActions.Player.SkillSlot4.performed -= _slot4;

        playerInputActions.Player.Movement.performed -= OnMovement;
        playerInputActions.Player.Run.performed -= OnRun;
        playerInputActions.Player.Run.canceled -= OnRun;
        playerInputActions.Player.BaseAttack.performed -= OnMovement;
        playerInputActions.Player.HealPotion.performed -= OnHealPotion;
        playerInputActions.Player.ManaPotion.performed -= OnManaPotion;

        playerInputActions.Dispose();
    }

    private void OnSkillPerformed(int slotId)
    {
        SkillPerformed?.Invoke(this, new SkillPerformedEventArgs { slotId = slotId });
    }
}