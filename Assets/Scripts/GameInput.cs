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

    public event EventHandler<SkillPerformedEventArgs> SkillPerformed;

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
    }

    private void OnDestroy()
    {
        playerInputActions.Player.SkillSlot1.performed -= _slot1;
        playerInputActions.Player.SkillSlot2.performed -= _slot2;
        playerInputActions.Player.SkillSlot3.performed -= _slot3;
        playerInputActions.Player.SkillSlot4.performed -= _slot4;

        playerInputActions.Dispose();
    }

    private void OnSkillPerformed(int slotId)
    {
        SkillPerformed?.Invoke(this, new SkillPerformedEventArgs { slotId = slotId });
    }
}