using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField]
    private float hp = 100f, mana = 100f;
    [SerializeField] private float maxHp, maxMana;

    public float HP => hp;
    public float Mana => mana;

    private Inventory inventory;
    private SkillSystem skillSystem;

    public event EventHandler PlayerHPChanged;
    public event EventHandler PlayerManaChanged;
    public event EventHandler PlayerTookDamage;
    public event EventHandler PlayerDied;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        inventory = GetComponent<Inventory>();
        skillSystem = GetComponent<SkillSystem>();
    }

    private void Start()
    {
        GameInput.Instance.SkillPerformed += OnSkillPerformed;
    }

    private void OnSkillPerformed(object sender, GameInput.SkillPerformedEventArgs e)
    {
        skillSystem.CastSkill(e.slotId);
    }

    private void ConsumeHealingPotion()
    {
        var hasPotion = inventory.TryConsumeHealingPotion();
        if (hasPotion)
        {
            var healAmount = inventory.GetHealingPotionHealthAmount();
            Heal(healAmount);
        }
    }

    private void ConsumeManaPotion()
    {
        var hasPotion = inventory.TryConsumeManaPotion();
        if (hasPotion)
        {
            var manaRecoverAmount = inventory.GetManaPotionRecoverAmount();
            RecoverMana(manaRecoverAmount);
        }
    }

    private void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            PlayerDied?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            PlayerTookDamage?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Heal(float healAmount)
    {
        if (hp == maxHp) return;
        hp = Mathf.Min(hp + healAmount, maxHp);
        PlayerHPChanged?.Invoke(this, EventArgs.Empty);
    }

    private void RecoverMana(float recoverAmount)
    {
        if (mana == maxMana) return;
        mana = Mathf.Min(mana + recoverAmount, maxMana);
        PlayerManaChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseMana(float manaUsageAmount)
    {
        mana = Mathf.Max(mana - manaUsageAmount, 0);
        PlayerManaChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Save(ref PlayerState playerState)
    {
        playerState.hp = hp;
        playerState.maxHp = maxHp;
        playerState.mana = mana;
        playerState.maxMana = maxMana;
    }

    public void Load(PlayerState playerState)
    {
        hp = playerState.hp;
        maxHp = playerState.maxHp;
        mana = playerState.mana;
        maxMana = playerState.maxMana;
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
}