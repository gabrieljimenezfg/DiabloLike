using System;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    [SerializeField] private SkillSO skillSO;

    public SkillSO SkillData => skillSO;
    public class SkillPerformedEventArgs : EventArgs
    {
        public SkillSO skillSO;
        public Vector3 position;
    }

    public static event EventHandler<SkillPerformedEventArgs> SkillPreCasted;
    public static event EventHandler<SkillPerformedEventArgs> SkillPerformed;

    protected virtual void Awake()
    {
        SkillPreCasted?.Invoke(this, new SkillPerformedEventArgs
        {
            skillSO = skillSO,
            position = transform.position,
        });
        gameObject.SetActive(false);
    }

    public virtual void StartCast()
    {
        gameObject.SetActive(true);
        SkillPerformed?.Invoke(this, new SkillPerformedEventArgs
        {
            skillSO = skillSO,
            position = transform.position
        });
    }
}