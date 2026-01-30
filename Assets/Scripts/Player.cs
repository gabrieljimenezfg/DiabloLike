using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private float hp, mana;
    [SerializeField] private float maxHp, maxMana;

    private Inventory inventory;

    public event EventHandler PlayerUsedPotion;
    public event EventHandler PlayerHealed;
    public event EventHandler PlayerRecoveredMana;
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
    }

    private void Start()
    {
        GameInput.Instance.SkillPerformed += OnSkillPerformed;
    }

    private void OnSkillPerformed(object sender, GameInput.SkillPerformedEventArgs e)
    {
        Debug.Log("Skill slot performed");
        Debug.Log(e.slotId);
    }


    private void Update()
    {
        TestSaveLoad();
    }

    private void TestSaveLoad()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ConsumeHealingPotion();
        }
    }

    private void ConsumeHealingPotion()
    {
        var hasPotion = inventory.PickupHealingPotion();
        if (hasPotion)
        {
            var healAmount = inventory.GetHealingPotionHealthAmount();
            Heal(healAmount);
            PlayerUsedPotion?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ConsumeManaPotion()
    {
        var hasPotion = inventory.PickupManaPotion();
        if (hasPotion)
        {
            var manaRecoverAmount = inventory.GetManaPotionRecoverAmount();
            RecoverMana(manaRecoverAmount);
            PlayerUsedPotion?.Invoke(this, EventArgs.Empty);
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
        hp += Mathf.Min(hp + healAmount, maxHp);
        PlayerHealed?.Invoke(this, EventArgs.Empty);
    }

    private void RecoverMana(float recoverAmount)
    {
        mana += Mathf.Min(mana + recoverAmount, maxMana);
        PlayerRecoveredMana?.Invoke(this, EventArgs.Empty);
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